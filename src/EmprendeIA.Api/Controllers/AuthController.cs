using EmprendeIA.Application.Users.Register;
using EmprendeIA.Application.Users.Login;
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
        var token = await _mediator.Send(command);
        return Ok(new { token });
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
    public IActionResult Me()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        return Ok(new
        {
            userId,
            email,
            role
        });
    }
    }