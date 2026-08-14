using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Repositories;

public interface IMediaListRepository
{
    Task<MediaList?> GetByIdAsync (int id);
    Task<IReadOnlyList<MediaList>> GetForUserAsync(int userId);
    Task AddAsync(MediaList mediaList);
    void Update(MediaList mediaList);
    void Delete(MediaList mediaList);
}