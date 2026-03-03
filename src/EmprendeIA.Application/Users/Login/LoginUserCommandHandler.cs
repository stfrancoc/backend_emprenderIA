using EmprendeIA.Domain.Interfaces;
using MediatR;
using BCrypt.Net;

namespace EmprendeIA.Application.Users.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
            throw new Exception("Credenciales inválidas.");

        var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!passwordValid)
            throw new Exception("Credenciales inválidas.");

        var token = _jwtService.GenerateToken(
            user.Id,
            user.Email,
            user.Role
        );

        return token;
    }
}