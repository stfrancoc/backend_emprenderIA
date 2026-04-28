using EmprendeIA.Domain.Entities;

namespace EmprendeIA.Domain.Interfaces;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetByUserIdAsync(Guid userId);
    Task AddAsync(UserProfile profile);
    Task UpdateAsync(UserProfile profile);
}
