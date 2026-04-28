using EmprendeIA.Domain.Entities;

namespace EmprendeIA.Domain.Interfaces;

public interface IProjectBmcRepository
{
    Task<ProjectBmc?> GetByProjectIdAsync(Guid projectId);
    Task AddAsync(ProjectBmc bmc);
    Task UpdateAsync(ProjectBmc bmc);
}
