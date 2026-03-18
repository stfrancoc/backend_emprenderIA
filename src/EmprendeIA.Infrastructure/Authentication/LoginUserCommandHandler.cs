using EmprendeIA.Domain.Interfaces;
using MediatR;
using BCrypt.Net;
using EmprendeIA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using EmprendeIA.Infrastructure.Persistence;
using EmprendeIA.Application.Users.Login;

namespace EmprendeIA.Infrastructure.Authentication;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtService _jwtService;

    public LoginUserCommandHandler(ApplicationDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user == null)
            throw new Exception("Usuario no encontrado");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new Exception("Credenciales inválidas");

        var accessToken = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };

        await _context.RefreshTokens.AddAsync(refreshTokenEntity);
        await _context.SaveChangesAsync(cancellationToken);

        return new LoginResponse(accessToken, refreshToken);
    }
}