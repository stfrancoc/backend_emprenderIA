using EmprendeIA.Domain.Entities;

namespace EmprendeIA.Domain.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
}