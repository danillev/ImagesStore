using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImagesStore.API.Models
{
    public record class Image
    {
        [Key]
        public int Id { get; set; } = default!;
        [Required]
        public string ImageName { get; set; }
        [Required]
        public string ImagePath { get; set; }
        [Required]
        public string ImageType { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
    }
}
