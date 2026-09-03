using Marquee.Application.DTOs.MediaList;
using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Repositories;

public interface IMediaListRepository
{
    Task<MediaList?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MediaList?> GetByIdWithItemsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync (int userId, string name, CancellationToken cancellationToken = default);
    Task AddAsync (MediaList mediaList, CancellationToken cancellationToken = default);
    void Update (MediaList mediaList);
    void Delete (MediaList mediaList);
    
    Task AddItemAsync(MediaListItem item, CancellationToken cancellationToken = default);
    void RemoveItem(MediaListItem item);
    Task<MediaListItem?> GetItemAsync(int mediaListId, int mediaId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MediaListDto>> GetUserListDtosAsync(int userId, int? mediaId, CancellationToken cancellationToken = default);
}