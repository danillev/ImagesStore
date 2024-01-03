using ImagesStore.API.Data;
using ImagesStore.API.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ImagesStore.API.Repositories
{
    public class UserGenericRepository : GenericRepository<User>
    {

        public UserGenericRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async ValueTask<User> GetByUserName(string username)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async ValueTask<User> GetByEmail(string email) 
        {
            return await _context.Set<User>().FirstOrDefaultAsync(x => x.Email == email);
        }

        public async ValueTask<bool> UserExists(int userId)
        {
            return _context.Set<User>().FindAsync(userId) != null;
        }
    }
}
