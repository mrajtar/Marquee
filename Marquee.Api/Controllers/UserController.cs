using AutoMapper;
using Marquee.Application.DTOs.User;
using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marquee.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService  _userService;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService  _currentUserService;

    public UserController(IUserService userService, IMapper mapper, ICurrentUserService currentUserService)
    {
        _userService = userService;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(id, cancellationToken);

        if (user == null)
        {
            return NotFound(new { message = $"User with ID {id} not found." });
        }
        return Ok(_mapper.Map<UserDetailsDto>(user));
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId == null)
            return Unauthorized();
        
        var user  = await _userService.GetByIdAsync(userId.Value, cancellationToken);
        
        if (user == null)
        {
            return NotFound(new { message = $"Current user was not found." });
        }
        
        return Ok(_mapper.Map<UserDetailsDto>(user));
    }

    [HttpPut("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId == null)
            return Unauthorized();
        
        var user = _mapper.Map<User>(dto);
        user.Id = userId.Value;

        try
        {
            await _userService.UpdateAsync(user, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteMe(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == null)
            return Unauthorized();

        try
        {
            await _userService.DeleteAsync(userId.Value, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}