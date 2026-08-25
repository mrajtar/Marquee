using System.Security.Claims;
using AutoMapper;
using Marquee.Application.DTOs.Rating;
using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marquee.Controllers;

[ApiController]
[Authorize]
[Route("api/media/{mediaId:int}/rating")]
public class RatingController : ControllerBase
{
    private readonly IRatingService _ratingService;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public RatingController(IRatingService ratingService, IMapper mapper, ICurrentUserService currentUserService)
    {
        _ratingService = ratingService;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(RatingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRating(int mediaId, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId is null)
            return Unauthorized();
        
        var rating = await _ratingService.GetAsync(userId.Value, mediaId, cancellationToken);

        if (rating is null)
        {
            return NotFound(new { message = "You hae not rated this media." });
        }
        
        return Ok(_mapper.Map<RatingDto>(rating));
    }

    [HttpPut]
    [ProducesResponseType(typeof(RatingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetRating(int mediaId, [FromBody] SetRatingDto dto,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        
        if (userId is null)
            return Unauthorized();

        try
        {
            var rating = await _ratingService.SetAsync(userId.Value, mediaId, dto.Value, cancellationToken);
            return Ok(_mapper.Map<RatingDto>(rating));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new { message = ex.Message});
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRating(int mediaId, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        
        if (userId is null)
            return Unauthorized();

        try
        {
            await _ratingService.DeleteAsync(userId.Value, mediaId, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}