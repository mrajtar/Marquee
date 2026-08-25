using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Marquee.Infrastructure.Authentication;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;

    public JwtTokenService(IConfiguration configuration, UserManager<User> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }
    
    public async Task<(string Token, DateTime ExpiresAt)> CreateTokenAsync(User user)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var key = jwtSection["Key"]
                  ?? throw new InvalidOperationException("JWT key is not configured.");
        var issuer = jwtSection["Issuer"]
                     ?? throw new InvalidOperationException("JWT issuer is not configured.");
        var audience = jwtSection["Audience"]
                       ?? throw new InvalidOperationException("JWT audience is not configured.");
        var expirationMinutes = jwtSection.GetValue<int>("ExpirationMinutes");
        var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName!)
        };

        foreach (var role in roles)
        {
            claims.Add(
                new Claim(ClaimTypes.Role, role));
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler()
            .WriteToken(token);
        
        return (tokenString, expiresAt);
    }
}