using AutoMapper;
using Marquee.Application.DTOs.Review;
using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marquee.Controllers;
[ApiController]
[Route("api")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly ICurrentUserService _currentUserService;

    public ReviewController(IReviewService reviewService, ICurrentUserService currentUserService)
    {
        _reviewService = reviewService;
        _currentUserService = currentUserService;
    }

    [HttpGet("media/{mediaId:int}/reviews")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<ReviewListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByMedia(int mediaId, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        var reviews = await _reviewService.GetByMediaIdAsync(mediaId, currentUserId, cancellationToken);
        return Ok(reviews);
    }

    [HttpGet("reviews/recent")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<ReviewListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecent([FromQuery] int count = 10, CancellationToken cancellationToken = default)
    {
        count = Math.Clamp(count, 1, 20);
        var currentUserId = _currentUserService.UserId;
        var reviews = await _reviewService.GetRecentAsync(currentUserId, count, cancellationToken);
        return Ok(reviews);
    }
    
    [HttpPost("media/{mediaId:int}/reviews")]
    [Authorize]
    [ProducesResponseType(typeof(ReviewListDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Add(int mediaId, [FromBody] CreateReviewDto dto, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId is null)
            return Unauthorized();

        try
        {
            var review = await _reviewService.CreateAsync(userId.Value, mediaId, dto.Content, dto.ContainsSpoilers, cancellationToken);
            var result = await _reviewService.GetDtoByIdAsync(review.Id, userId.Value, cancellationToken);
            return CreatedAtAction(nameof(GetByMedia), new { mediaId }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("reviews/{reviewId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int reviewId, [FromBody] UpdateReviewDto dto, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId is null)
            return Unauthorized();

        try
        {
            await _reviewService.UpdateAsync(userId.Value, reviewId, dto.Content, dto.ContainsSpoilers, cancellationToken);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpDelete("reviews/{reviewId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int reviewId, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId is null) return Unauthorized();

        try
        {
            await _reviewService.DeleteAsync(userId.Value, reviewId, cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}