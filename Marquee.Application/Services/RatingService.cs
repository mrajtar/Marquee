using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Repositories;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;

namespace Marquee.Application.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RatingService(IRatingRepository ratingRepository, IUnitOfWork unitOfWork)
    {
        _ratingRepository = ratingRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Rating?> GetAsync(int userId, int mediaId, CancellationToken cancellationToken)
    {
        return await _ratingRepository.GetAsync(userId, mediaId, cancellationToken);
    }

    public async Task<Rating> SetAsync(int userId, int mediaId, int value, CancellationToken cancellationToken)
    {
        if (value is < 1 or > 20)
        {
            throw new ArgumentOutOfRangeException(nameof(value),"Rating must be between 1 and 10");
        }
        var existingRating = await _ratingRepository.GetAsync(userId, mediaId, cancellationToken);
        if (existingRating is not null)
        {
            existingRating.Value = value;
            existingRating.UpdatedAt = DateTime.UtcNow;
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return existingRating;
        }

        var rating = new Rating
        {
            UserId = userId,
            MediaId = mediaId,
            Value = value,
            CreatedAt = DateTime.UtcNow
        };
        
        await _ratingRepository.AddAsync(rating, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return rating;
    }

    public async Task DeleteAsync(int userId, int mediaId, CancellationToken cancellationToken)
    {
        var rating = await _ratingRepository.GetAsync(userId, mediaId, cancellationToken);

        if (rating is null)
        {
            throw new KeyNotFoundException("Rating was not found");
        }
        
        _ratingRepository.Delete(rating);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}