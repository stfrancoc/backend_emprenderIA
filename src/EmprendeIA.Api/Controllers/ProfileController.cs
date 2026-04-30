using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EmprendeIA.Application.Users.Profile;

namespace EmprendeIA.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    /// <summary>
    /// Get the authenticated user's profile
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = GetUserId();
        var profile = await _mediator.Send(new GetUserProfileQuery(userId));

        if (profile == null) return NotFound("Perfil no encontrado");

        return Ok(profile);
    }

    /// <summary>
    /// Get a public profile by user ID
    /// </summary>
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetProfile(Guid userId)
    {
        var profile = await _mediator.Send(new GetUserProfileQuery(userId));

        if (profile == null) return NotFound("Perfil no encontrado");

        return Ok(profile);
    }

    /// <summary>
    /// Create or update the authenticated user's profile
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateProfile(UpdateUserProfileCommand command)
    {
        command.UserId = GetUserId();
        var result = await _mediator.Send(command);

        if (result == null) return NotFound("Usuario no encontrado");

        return Ok(result);
    }
}
