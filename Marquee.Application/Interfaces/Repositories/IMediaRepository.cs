using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Repositories;

public interface IMediaRepository
{
    Task<Media?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Media?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Media>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Media>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<(double? AverageRating, int RatingCount)> GetRatingSummaryAsync(int mediaId , CancellationToken cancellationToken = default);
    Task<int> GetReviewCountAsync(int mediaId, CancellationToken cancellationToken = default);
    Task AddAsync(Media media, CancellationToken cancellationToken = default);
    void Update(Media media);
    void Delete(Media media);
}