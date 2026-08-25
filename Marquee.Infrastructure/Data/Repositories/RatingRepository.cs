using Marquee.Application.Interfaces.Repositories;
using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marquee.Infrastructure.Data.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly MarqueeDbContext _context;
    
    public RatingRepository(MarqueeDbContext context)
    {
        _context = context;
    }
    
    public async Task<Rating?> GetAsync(int userId, int mediaId, CancellationToken cancellationToken)
    {
        return await _context.Ratings
            .FirstOrDefaultAsync(r => r.UserId ==  userId && r.MediaId == mediaId, cancellationToken);
    }

    public async Task AddAsync(Rating rating, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(rating);
        
        await _context.Ratings.AddAsync(rating, cancellationToken);
    }

    public void Delete(Rating rating)
    {
        ArgumentNullException.ThrowIfNull(rating);
        
        _context.Ratings.Remove(rating);
    }
}