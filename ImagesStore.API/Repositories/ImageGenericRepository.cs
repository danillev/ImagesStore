using ImagesStore.API.Data;
using ImagesStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ImagesStore.API.Repositories
{
    public class ImageGenericRepository: GenericRepository<Image>
    {
        public ImageGenericRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        
        public async ValueTask<IEnumerable<Image>> GetByUserId(string userId)
        {
            return _context.Set<Image>().ToListAsync().Result.Where(x => x.UserId == userId);
        }
    }
}
