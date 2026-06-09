using EmprendeIA.Domain.Entities;

namespace EmprendeIA.Domain.Interfaces;

public interface IMilestoneRepository
{
    Task<Milestone?> GetByIdAsync(Guid id);
    Task<IEnumerable<Milestone>> GetByProjectIdAsync(Guid projectId);
    Task AddAsync(Milestone milestone);
    Task UpdateAsync(Milestone milestone);
    Task DeleteAsync(Guid id);
}
