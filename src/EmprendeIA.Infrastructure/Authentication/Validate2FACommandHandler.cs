using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Entities;
using EmprendeIA.Application.Users.TwoFactor;
using EmprendeIA.Application.Users.Login;
using EmprendeIA.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using OtpNet;

namespace EmprendeIA.Infrastructure.Authentication;

public class Validate2FACommandHandler : IRequestHandler<Validate2FACommand, LoginResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtService _jwtService;

    public Validate2FACommandHandler(ApplicationDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse> Handle(Validate2FACommand request, CancellationToken cancellationToken)
    {
        // The temp token is a signed JWT with short expiry containing the user ID
        // Parse it to get user ID
        Guid userId;
        try
        {
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(request.TempToken);
            var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (userIdClaim == null)
                throw new Exception("Token temporal inválido");
            userId = Guid.Parse(userIdClaim);
        }
        catch
        {
            throw new Exception("Token temporal inválido o expirado");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            throw new Exception("Usuario no encontrado");

        if (string.IsNullOrEmpty(user.TotpSecret))
            throw new Exception("2FA no está configurado");

        // Verify TOTP code
        var secretBytes = Base32Encoding.ToBytes(user.TotpSecret);
        var totp = new Totp(secretBytes);
        var isValid = totp.VerifyTotp(request.Code, out _, VerificationWindow.RfcSpecifiedNetworkDelay);

        if (!isValid)
            throw new Exception("Código 2FA inválido");

        // Generate full tokens
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
