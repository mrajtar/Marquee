using System.Security.Claims;
using Marquee.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Marquee.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated =>  _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public int? UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return int.TryParse(claim?.Value, out var userId) ? userId : null;
        }
    }
    
    public bool IsAdmin => _httpContextAccessor.HttpContext?.User.IsInRole("Admin") == true;
}