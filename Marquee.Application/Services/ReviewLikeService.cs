using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Repositories;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;

namespace Marquee.Application.Services;

public class ReviewLikeService : IReviewLikeService
{
    private readonly IReviewLikeRepository _reviewLikeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReviewLikeService(IReviewLikeRepository reviewLikeRepository, IUnitOfWork unitOfWork)
    {
        _reviewLikeRepository = reviewLikeRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<bool> IsLikedAsync(int userId, int reviewId, CancellationToken cancellationToken = default)
    {
        return await _reviewLikeRepository.ExistsAsync(userId, reviewId, cancellationToken);
    }

    public async Task<int> GetCountAsync(int reviewId, CancellationToken cancellationToken = default)
    {
        return await _reviewLikeRepository.GetCountAsync(reviewId, cancellationToken);
    }

    public async Task LikeAsync(int userId, int reviewId, CancellationToken cancellationToken = default)
    {
        var existingLike = await _reviewLikeRepository.GetAsync(userId, reviewId, cancellationToken);
        if (existingLike != null)
            return;
        var reviewLike = new ReviewLike
        {
            UserId = userId,
            ReviewId = reviewId,
            CreatedAt = DateTime.UtcNow
        };
        
        await _reviewLikeRepository.AddAsync(reviewLike, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UnlikeAsync(int userId, int reviewId, CancellationToken cancellationToken = default)
    {
        var existingLike = await _reviewLikeRepository.GetAsync(userId, reviewId, cancellationToken);
        if (existingLike == null)
            return;
        _reviewLikeRepository.Delete(existingLike);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}