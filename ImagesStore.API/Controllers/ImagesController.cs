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
            if(_userRepository.GetById(userId) == null)
            {
                return Ok("User not found");
            }

            if(files == null ||  files.Count == 0)
            {
                return BadRequest("No files uploaded");
            }

            List<string> errors = new List<string>();
            foreach (var file in files)
            {
                if (!IsImage(Path.GetExtension(file.FileName)))
                {
                    errors.Add($"File '{file.FileName}' is not an image");
                    continue;
                }
                if (file.Length > 10 * 1024 * 1024)
                {
                    errors.Add($"File '{file.FileName}' is too large. Maximum allowed size is 10 MB");
                    continue;
                }

                string imagePath = Path.Combine("Images", userId.ToString(), Path.GetFileName(file.FileName));
                string physicalPath = Path.Combine(Directory.GetCurrentDirectory(), imagePath);
                using (var fileStreamer = new FileStream(physicalPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStreamer).ConfigureAwait(false);
                }

                Image image = new Image
                {
                    ImageName = file.Name,
                    UserId = userId.ToString(),
                    ImageType = file.ContentType,
                    ImagePath = Path.Combine("Images", userId.ToString(), imagePath),
                };

                _imageRepository.Create(image);
                _imageRepository.Save();
            }
            if(errors.Count > 0) { return BadRequest(errors); }
            return Ok();
        }

        private bool IsImage (string fileExtension)
        {
            string[] imageExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".webp" };
            return imageExtensions.Contains(fileExtension);
        }



        [HttpGet("{id}")]
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
