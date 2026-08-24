using Marquee.Application.Interfaces.Repositories;
using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marquee.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MarqueeDbContext _context;

    public UserRepository(MarqueeDbContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .OrderBy(u => u.UserName)
            .ToListAsync(cancellationToken);
    }

    public void Delete(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        _context.Users.Remove(user);
    }
}