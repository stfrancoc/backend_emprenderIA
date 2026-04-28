using EmprendeIA.Domain.Entities;
using EmprendeIA.Domain.Interfaces;
using MediatR;
using BCrypt.Net;

namespace EmprendeIA.Application.Users.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);

        if (existingUser is not null)
            throw new Exception("El usuario ya existe.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User(
            request.Name,
            request.Email,
            passwordHash,
            request.Role
        );

        await _userRepository.AddAsync(user);

        return user.Id;
    }
}