using Microsoft.EntityFrameworkCore;
using Web.Api.Users;

namespace Web.Api.Database;

public sealed class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<bool> Exists(string email)
    {
        return await dbContext.Users.AnyAsync(u => u.Email == email);
    }

    public async Task Insert(User user)
    {
        dbContext.Users.Add(user);

        await dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
    }
}
