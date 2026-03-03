using EmprendeIA.Domain.Projects;

namespace EmprendeIA.Domain.Interfaces;

public interface IProjectRepository
{
    Task AddAsync(Project project);
    Task<Project?> GetByIdAsync(Guid id);
}