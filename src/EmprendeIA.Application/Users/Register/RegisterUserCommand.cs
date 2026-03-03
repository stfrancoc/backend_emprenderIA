using MediatR;

namespace EmprendeIA.Application.Users.Register;

public record RegisterUserCommand(
    string Email,
    string Password,
    string Role
) : IRequest<Guid>;