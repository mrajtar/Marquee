using Marquee.Application.DTOs.Media;
using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Services;

public interface IMediaService
{
    Task<Media?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Media?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Media>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MediaListDto>> GetAllAsListAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Media>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<(double? AverageRating, int RatingCount)> GetRatingSummaryAsync(int mediaId, CancellationToken cancellationToken = default);
    Task<int> GetReviewCountAsync(int mediaId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MediaListDto>> GetTrendingAsync(int count, CancellationToken cancellationToken = default);
    Task<MediaListDto?> GetFeaturedAsync(CancellationToken cancellationToken = default);
    Task<Media> AddAsync(Media media, IReadOnlyCollection<int> genreIds, IReadOnlyCollection<int> keywordIds, CancellationToken cancellationToken = default);
    Task UpdateAsync(Media media, IReadOnlyCollection<int> genreIds, IReadOnlyCollection<int> keywordIds, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}