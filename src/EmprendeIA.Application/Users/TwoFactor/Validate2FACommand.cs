using MediatR;
using EmprendeIA.Application.Users.Login;

namespace EmprendeIA.Application.Users.TwoFactor;

/// <summary>
/// Validates TOTP code during login when 2FA is enabled.
/// Uses the temp token from the initial login step.
/// </summary>
public record Validate2FACommand(string TempToken, string Code) : IRequest<LoginResponse>;
