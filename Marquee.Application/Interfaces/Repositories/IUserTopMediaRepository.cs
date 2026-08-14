using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Repositories;

public interface IUserTopMediaRepository
{
    Task<IReadOnlyList<UserTopMedia>> GetForUserAsync (int userId);
    Task AddAsync (UserTopMedia topMedia);
    Task Update (UserTopMedia topMedia);
    Task Delete (UserTopMedia topMedia);
}