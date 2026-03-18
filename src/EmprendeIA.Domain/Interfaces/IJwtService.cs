namespace EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Entities;

public interface IJwtService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
}