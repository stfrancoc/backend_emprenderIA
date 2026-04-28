using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Application.Users.TwoFactor;
using OtpNet;

namespace EmprendeIA.Infrastructure.Authentication;

public class VerifySetup2FACommandHandler : IRequestHandler<VerifySetup2FACommand, bool>
{
    private readonly IUserRepository _userRepository;

    public VerifySetup2FACommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(VerifySetup2FACommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new Exception("Usuario no encontrado");

        if (string.IsNullOrEmpty(user.TotpSecret))
            throw new Exception("No se ha iniciado la configuración 2FA");

        // Verify the TOTP code
        var secretBytes = Base32Encoding.ToBytes(user.TotpSecret);
        var totp = new Totp(secretBytes);
        var isValid = totp.VerifyTotp(request.Code, out _, VerificationWindow.RfcSpecifiedNetworkDelay);

        if (!isValid)
            throw new Exception("Código 2FA inválido");

        // 2FA is already enabled from setup, just confirm it's valid
        return true;
    }
}
