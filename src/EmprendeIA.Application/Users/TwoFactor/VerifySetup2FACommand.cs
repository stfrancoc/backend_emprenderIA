using MediatR;

namespace EmprendeIA.Application.Users.TwoFactor;

/// <summary>
/// Verifies the first TOTP code after setup and enables 2FA on the user account.
/// </summary>
public record VerifySetup2FACommand(Guid UserId, string Code) : IRequest<bool>;
