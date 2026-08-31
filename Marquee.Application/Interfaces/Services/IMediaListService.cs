using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Services;

public interface IMediaListService
{
    Task<IReadOnlyList<MediaList>> GetUserListsAsync(int userId, CancellationToken cancellationToken = default);
    Task<MediaList?> GetByIdWithItemsAsync(int id, CancellationToken cancellationToken = default);
    Task<MediaList> CreateAsync(MediaList mediaList, CancellationToken cancellationToken = default);
    Task UpdateAsync(MediaList mediaList, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task AddItemAsync(int mediaListId, int mediaId, CancellationToken cancellationToken = default);
    Task RemoveItemAsync(int mediaListId, int mediaId, CancellationToken cancellationToken = default);
}