using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Application.Users.TwoFactor;
using OtpNet;

namespace EmprendeIA.Infrastructure.Authentication;

public class Setup2FACommandHandler : IRequestHandler<Setup2FACommand, Setup2FAResponse>
{
    private readonly IUserRepository _userRepository;

    public Setup2FACommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Setup2FAResponse> Handle(Setup2FACommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new Exception("Usuario no encontrado");

        if (user.Is2FAEnabled)
            throw new Exception("2FA ya está habilitado");

        // Generate a new TOTP secret
        var secretKey = KeyGeneration.GenerateRandomKey(20);
        var base32Secret = Base32Encoding.ToString(secretKey);

        // Store the secret temporarily (will be persisted on verify)
        user.Enable2FA(base32Secret);
        await _userRepository.UpdateAsync(user);

        // Generate the provisioning URI for QR code (compatible with Google Authenticator)
        var qrUri = $"otpauth://totp/EmprendeIA:{user.Email}?secret={base32Secret}&issuer=EmprendeIA&digits=6&period=30";

        return new Setup2FAResponse(base32Secret, qrUri);
    }
}
