using MediatR;

namespace EmprendeIA.Application.Users.TwoFactor;

/// <summary>
/// Disables 2FA on user account. Requires current password and TOTP code.
/// </summary>
public record Disable2FACommand(Guid UserId, string Password, string Code) : IRequest<bool>;
