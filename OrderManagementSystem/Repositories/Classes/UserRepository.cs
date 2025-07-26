using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.Interfaces;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(OrderManagementDbContext context) : base(context) { }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
}
