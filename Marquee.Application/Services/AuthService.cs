using AutoMapper;
using Marquee.Application.DTOs.Auth;
using Marquee.Application.DTOs.User;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Marquee.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager,
        IJwtTokenService jwtTokenService, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userManager.FindByNameAsync(dto.Username);
        if (existingUser is not null)
        {
            throw new InvalidOperationException("Username is already taken.");
        }
        
        var existingEmail = await _userManager.FindByEmailAsync(dto.Email);
        if (existingEmail is not null)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        var user = new User
        {
            UserName = dto.Username,
            Email = dto.Email,
            DisplayName = dto.DisplayName,
            CreatedAt = DateTime.UtcNow
        };
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            
            throw new InvalidOperationException(errors);
        }

        await _userManager.AddToRoleAsync(user, "User");
        var (token, expiresAt) = await _jwtTokenService.CreateTokenAsync(user);

        return new AuthResponseDto
        {
            AccessToken = token,
            ExpiresAt = expiresAt,
            User = _mapper.Map<UserDetailsDto>(user)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByNameAsync(dto.Username);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }
        
        var (token, expiresAt) = await _jwtTokenService.CreateTokenAsync(user);

        return new AuthResponseDto
        {
            AccessToken = token,
            ExpiresAt = expiresAt,
            User = _mapper.Map<UserDetailsDto>(user)
        };
    }

    public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(errors);
        }
    }
}