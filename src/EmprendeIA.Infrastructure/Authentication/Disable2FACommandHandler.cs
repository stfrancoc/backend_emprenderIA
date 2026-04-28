using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Application.Users.TwoFactor;
using OtpNet;

namespace EmprendeIA.Infrastructure.Authentication;

public class Disable2FACommandHandler : IRequestHandler<Disable2FACommand, bool>
{
    private readonly IUserRepository _userRepository;

    public Disable2FACommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(Disable2FACommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new Exception("Usuario no encontrado");

        if (!user.Is2FAEnabled)
            throw new Exception("2FA no está habilitado");

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new Exception("Contraseña inválida");

        // Verify current TOTP code
        if (string.IsNullOrEmpty(user.TotpSecret))
            throw new Exception("Error interno: secret no encontrado");

        var secretBytes = Base32Encoding.ToBytes(user.TotpSecret);
        var totp = new Totp(secretBytes);
        var isValid = totp.VerifyTotp(request.Code, out _, VerificationWindow.RfcSpecifiedNetworkDelay);

        if (!isValid)
            throw new Exception("Código 2FA inválido");

        user.Disable2FA();
        await _userRepository.UpdateAsync(user);

        return true;
    }
}
