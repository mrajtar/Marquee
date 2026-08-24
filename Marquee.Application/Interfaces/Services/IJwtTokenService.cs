using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Services;

public interface IJwtTokenService
{
    Task<(string Token, DateTime ExpiresAt)> CreateTokenAsync(User user);
}