using EmprendeIA.Domain.Projects;

namespace EmprendeIA.Domain.Interfaces;

public interface IProjectRepository
{
    Task AddAsync(Project project);
    Task<Project?> GetByIdAsync(Guid id);
    Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId); 
    Task UpdateAsync(Project project); 
    Task DeleteAsync(Project project);
}