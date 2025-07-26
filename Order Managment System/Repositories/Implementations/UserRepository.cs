using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly OrderManagementDbContext _context;

        public UserRepository(OrderManagementDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}