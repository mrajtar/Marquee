using Marquee.Application.DTOs.Review;
using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Repositories;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;

namespace Marquee.Application.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork  _unitOfWork;
    private readonly IMediaRepository _mediaRepository;

    public ReviewService(IReviewRepository reviewRepository, IUnitOfWork unitOfWork, IMediaRepository mediaRepository)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _mediaRepository = mediaRepository;
    }
    
    public async Task<IReadOnlyList<ReviewListDto>> GetByMediaIdAsync(int mediaId, int? currentUserId, CancellationToken cancellationToken = default)
    {
        return await _reviewRepository.GetByMediaIdAsync(mediaId, currentUserId, cancellationToken);
    }

    public async Task<ReviewListDto?> GetDtoByIdAsync(int reviewId, int? currentUserId, CancellationToken cancellationToken = default)
    {
        return await _reviewRepository.GetDtoByIdAsync(reviewId, currentUserId, cancellationToken);
    }

    public async Task<IReadOnlyList<ReviewListDto>> GetRecentAsync(int? currentUserId, int count, CancellationToken cancellationToken = default)
    {
        return await _reviewRepository.GetRecentAsync(currentUserId, count, cancellationToken);
    }

    public async Task<ReviewListDto> CreateAsync(int userId, int mediaId, string content, bool containsSpoilers, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Review cannot be empty", nameof(content));
        }
        
        var media = await _mediaRepository.GetByIdAsync(mediaId, cancellationToken);
        if (media is null)
            throw new KeyNotFoundException($"Media with ID {mediaId} was not found.");

        content = content.Trim();
        
        var review = new Review
        {
            UserId = userId,
            MediaId = mediaId,
            Content = content,
            ContainsSpoilers = containsSpoilers,
            CreatedAt = DateTime.UtcNow
        };
        await _reviewRepository.AddAsync(review, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var result = await _reviewRepository.GetDtoByIdAsync(review.Id, userId, cancellationToken);
        
        return result ?? throw new InvalidOperationException("Review could not be retrieved.");
    }

    public async Task UpdateAsync(int userId, int reviewId, string content, bool containsSpoilers, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Review content cannot be empty.", nameof(content));
        }

        content = content.Trim();

        var review = await _reviewRepository.GetByIdAsync(reviewId, cancellationToken);

        if (review is null)
        {
            throw new KeyNotFoundException($"Review with ID {reviewId} was not found.");
        }

        if (review.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only update your own reviews.");
        }

        review.Content = content;
        review.ContainsSpoilers = containsSpoilers;
        review.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int userId, int reviewId, CancellationToken cancellationToken = default)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId, cancellationToken);

        if (review is null)
        {
            throw new KeyNotFoundException($"Review with ID {reviewId} was not found.");
        }

        if (review.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only delete your own reviews.");
        }

        _reviewRepository.Delete(review);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}