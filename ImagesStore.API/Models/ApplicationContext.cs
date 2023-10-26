using Microsoft.EntityFrameworkCore;

namespace ImagesStore.API.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        { }
        public DbSet<User> Users { get; set; }
    }
}
