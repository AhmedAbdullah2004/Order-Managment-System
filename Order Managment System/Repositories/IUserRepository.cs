using OrderManagementSystem.Models;

namespace OrderManagementSystem.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}
