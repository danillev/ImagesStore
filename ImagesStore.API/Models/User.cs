using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ImagesStore.API.Models
{
    
    [Index("Email", IsUnique = true)]
    public class User : IdentityUser<int>
    {
        [Required]
        [EmailAddress]
        public override string Email { get; set; }

    }
    
}
