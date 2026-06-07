using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmprendeIA.Domain.Projects;

namespace EmprendeIA.Domain.Interfaces;

public interface IProjectMatchRepository
{
    Task<ProjectMatch?> GetByIdAsync(Guid id);
    Task<ProjectMatch?> GetMatchAsync(Guid projectId, Guid investorId);
    Task<List<ProjectMatch>> GetMatchesByUserAsync(Guid userId);
    Task<List<ProjectMatch>> GetMatchesByProjectAsync(Guid projectId);
    Task AddAsync(ProjectMatch match);
}
