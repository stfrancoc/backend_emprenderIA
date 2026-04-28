using MediatR;

namespace EmprendeIA.Application.Users.TwoFactor;

/// <summary>
/// Generates a new TOTP secret and returns the provisioning URI for QR code.
/// Requires authenticated user.
/// </summary>
public record Setup2FACommand(Guid UserId) : IRequest<Setup2FAResponse>;

public record Setup2FAResponse(string Secret, string QrUri);
