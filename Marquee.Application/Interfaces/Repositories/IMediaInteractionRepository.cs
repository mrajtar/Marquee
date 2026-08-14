using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Repositories;

public interface IMediaInteractionRepository
{
    Task <IReadOnlyList<MediaInteraction>> GetForUserAsync (int userId);
    Task<IReadOnlyList<MediaInteraction>> GetForUserAndMediaAsync (int userId, int mediaId);
    Task AddAsync(MediaInteraction mediaInteraction);
}