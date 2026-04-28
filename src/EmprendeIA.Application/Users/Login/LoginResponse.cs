using MediatR;

namespace EmprendeIA.Application.Users.Login;

public record LoginResponse(string AccessToken, string RefreshToken, bool Requires2FA = false, string? TempToken = null);