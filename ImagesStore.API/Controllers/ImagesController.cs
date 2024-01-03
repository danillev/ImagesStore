using ImagesStore.API.Data;
using ImagesStore.API.Interfaces;
using ImagesStore.API.Models;
using ImagesStore.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace ImagesStore.API.Controllers
{
    [Route("api/user={userId}/[controller]")]
    [ApiController]
    public class ImagesController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly ImageGenericRepository _imageRepository;
        private readonly UserGenericRepository _userRepository;

        public ImagesController(ApplicationContext context, IFileProvider fileProvider)
        {
            _context = context;
            _imageRepository = new ImageGenericRepository(_context);
            _userRepository = new UserGenericRepository(context);
            _fileProvider = fileProvider;
        }

        [HttpPost]
        public async Task<ActionResult> Post(IFormFileCollection files, int userId)
        {
            if (!_userRepository.UserExists(userId).Result)
            {
                return Ok("User not found");
            }

            if (files == null || files.Count == 0)
            {
                return BadRequest("No files uploaded");
            }

            List<string> errors = await ProcessFiles(files, userId);

            if (errors.Count > 0)
            {
                return BadRequest(errors);
            }

            return Ok();
        }

        private async Task<List<string>> ProcessFiles(IFormFileCollection files, int userId)
        {
            List<string> errors = new List<string>();

            foreach (var file in files)
            {
                errors.AddRange(await ProcessFile(file, userId));
            }

            return errors;
        }

        private async ValueTask<List<string>> ProcessFile(IFormFile file, int userId)
        {
            List<string> errors = new List<string>();

            if (!IsImage(file.FileName))
            {
                errors.Add($"File '{file.FileName}' is not an image");
                return errors;
            }

            if (file.Length > 10 * 1024 * 1024)
            {
                errors.Add($"File '{file.FileName}' is too large. Maximum allowed size is 10 MB");
                return errors;
            }

            string imagePath = SaveImage(file, userId);
            Image image = new Image(file, userId, imagePath);

            _imageRepository.Create(image);
            await _imageRepository.Save();

            return errors;
        }

        private string SaveImage(IFormFile file, int userId)
        {
            string imagePath = Path.Combine("Images", userId.ToString(), Path.GetFileName(file.FileName));
            string physicalPath = Path.Combine(Directory.GetCurrentDirectory(), imagePath);
            string directory = Path.GetDirectoryName(physicalPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var fileStreamer = new FileStream(physicalPath, FileMode.Create))
            {
                file.CopyTo(fileStreamer);
            }

            return imagePath;
        }

        private bool IsImage(string fileName)
        {
            string[] imageExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".webp" };
            string fileExtension = Path.GetExtension(fileName).ToLower();
            return imageExtensions.Contains(fileExtension);
        }



        [HttpGet]
        public async Task<ActionResult> GetImagesByUserId(string userId)
        {
            IEnumerable<Image> images = await _imageRepository.GetByUserId(userId);
            if (images == null )
            {
                return Ok(StatusCodes.Status204NoContent);
            }

            var imageFiles = new List<IFormFile>();
            foreach (var image in images)
            {
                var file = CreateFormFile(image);
                imageFiles.Add(file);
            }

            return Ok(imageFiles);
        }

        private IFormFile CreateFormFile(Image image)
        {
            using (var stream = new FileStream(image.ImagePath, FileMode.Open))
            {
                var file = new FormFile(stream, 0, stream.Length, image.ImageName, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = image.ImageType
                };

                return file;
            }
        }
    }
}
