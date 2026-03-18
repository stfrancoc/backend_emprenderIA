using MediatR;

namespace EmprendeIA.Application.Users.Login;

public record LoginUserCommand(string Email, string Password) : IRequest<LoginResponse>;