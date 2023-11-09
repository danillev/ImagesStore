using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImagesStore.API.Models
{
    public class Image
    {
        [Key]
        public string Id { get; set; } = default!;
        [Required]
        public string ImageBase64 { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
    }
}
