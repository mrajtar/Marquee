using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Repositories;

public interface IUserTopMediaRepository
{
    Task<IReadOnlyList<UserTopMedia>> GetForUserAsync (int userId);
    Task AddAsync (UserTopMedia topMedia);
    void Update (UserTopMedia topMedia);
    void Delete (UserTopMedia topMedia);
}