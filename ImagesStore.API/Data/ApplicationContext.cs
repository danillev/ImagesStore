using ImagesStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ImagesStore.API.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.Migrate();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
