using EmprendeIA.Application.Users.Register;
using EmprendeIA.Application.Users.Login;
using EmprendeIA.Application.Users.TwoFactor;
using EmprendeIA.Application.Users.Profile;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprendeIA.Infrastructure.Persistence;
using EmprendeIA.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace EmprendeIA.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthController(IMediator mediator, ApplicationDbContext context, IJwtService jwtService)
    {
        _mediator = mediator;
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var userId = await _mediator.Send(command);

        return Ok(new { UserId = userId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand command)
    {
        var response = await _mediator.Send(command);

        if (response.Requires2FA)
        {
            return Ok(new
            {
                requires2FA = true,
                tempToken = response.TempToken
            });
        }

        return Ok(new
        {
            accessToken = response.AccessToken,
            refreshToken = response.RefreshToken
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshToken && !x.IsRevoked);

        if (token == null || token.ExpiresAt < DateTime.UtcNow)
            return Unauthorized();

        var user = await _context.Users.FindAsync(token.UserId);

        var newAccessToken = _jwtService.GenerateToken(user!);

        return Ok(new
        {
            accessToken = newAccessToken
        });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdStr == null) return Unauthorized();

        var userId = Guid.Parse(userIdStr);
        var profile = await _mediator.Send(new GetUserProfileQuery(userId));

        if (profile == null) return NotFound();

        return Ok(profile);
    }

    // =========================
    // 2FA ENDPOINTS
    // =========================

    [HttpPost("2fa/setup")]
    [Authorize]
    public async Task<IActionResult> Setup2FA()
    {
        var userId = GetUserId();
        var result = await _mediator.Send(new Setup2FACommand(userId));
        return Ok(result);
    }

    [HttpPost("2fa/verify-setup")]
    [Authorize]
    public async Task<IActionResult> VerifySetup2FA([FromBody] Verify2FARequest request)
    {
        var userId = GetUserId();
        var result = await _mediator.Send(new VerifySetup2FACommand(userId, request.Code));
        return Ok(new { success = result });
    }

    [HttpPost("2fa/validate")]
    public async Task<IActionResult> Validate2FA([FromBody] Validate2FARequest request)
    {
        var response = await _mediator.Send(new Validate2FACommand(request.TempToken, request.Code));
        return Ok(new
        {
            accessToken = response.AccessToken,
            refreshToken = response.RefreshToken
        });
    }

    [HttpPost("2fa/disable")]
    [Authorize]
    public async Task<IActionResult> Disable2FA([FromBody] Disable2FARequest request)
    {
        var userId = GetUserId();
        var result = await _mediator.Send(new Disable2FACommand(userId, request.Password, request.Code));
        return Ok(new { success = result });
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }
}

// Request DTOs for 2FA endpoints
public record Verify2FARequest(string Code);
public record Validate2FARequest(string TempToken, string Code);
public record Disable2FARequest(string Password, string Code);