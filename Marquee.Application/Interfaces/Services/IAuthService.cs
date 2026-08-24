using Marquee.Application.DTOs.Auth;

namespace Marquee.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default);
    Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
    Task ChangePasswordAsync(int userId, ChangePasswordDto dto, CancellationToken cancellationToken = default);
}