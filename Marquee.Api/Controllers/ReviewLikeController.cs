using Marquee.Application.DTOs.User;
using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marquee.Controllers;
[ApiController]
[Authorize]
[Route("api/reviews/{reviewId:int}/like")]
public class ReviewLikeController : ControllerBase
{
    private readonly IReviewLikeService _reviewLikeService;
    private readonly ICurrentUserService  _currentUserService;

    public ReviewLikeController(IReviewLikeService reviewLikeService, ICurrentUserService currentUserService)
    {
        _reviewLikeService = reviewLikeService;
        _currentUserService = currentUserService;
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Like(int reviewId, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId is null)
            return Unauthorized();
        await _reviewLikeService.LikeAsync(userId.Value, reviewId, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Unlike(int reviewId, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId is null) 
            return Unauthorized();
        
        await _reviewLikeService.UnlikeAsync(userId.Value, reviewId, cancellationToken);
        return NoContent();
    }
}