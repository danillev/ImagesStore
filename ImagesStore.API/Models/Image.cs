using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImagesStore.API.Models
{
    public record class Image
    {
        [Key]
        public int Id { get; init; } = default!;
        [Required]
        public string ImageName { get; init; }
        [Required]
        public string ImagePath { get; init; }
        [Required]
        public string ImageType { get; init; }
        [ForeignKey("User")]
        public string UserId { get; init; }


        public Image()
        {

        }

        public Image(IFormFile file, int userId, string imagePath)
        {
            ImageName = file.Name;
            UserId = userId.ToString();
            ImageType = file.ContentType;
            ImagePath = imagePath;
        }
    }
        
}
