using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Repositories;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;

namespace Marquee.Application.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork  _unitOfWork;

    public ReviewService(IReviewRepository reviewRepository, IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IReadOnlyList<Review>> GetByMediaIdAsync(int mediaId, CancellationToken cancellationToken = default)
    {
        return await _reviewRepository.GetByMediaIdAsync(mediaId, cancellationToken);
    }

    public async Task<Review> CreateAsync(int userId, int mediaId, string content, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Review cannot be empty", nameof(content));
        }

        content = content.Trim();

        var review = new Review
        {
            UserId = userId,
            MediaId = mediaId,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };
        await _reviewRepository.AddAsync(review, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return review;
    }

    public async Task UpdateAsync(int userId, int reviewId, string content, CancellationToken cancellationToken = default)
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