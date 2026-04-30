using EmprendeIA.Domain.Entities;

namespace EmprendeIA.Domain.Interfaces;

public interface IFinancialRepository
{
    Task<ProjectFinancialAnalysis?> GetByProjectIdAsync(Guid projectId);
    Task AddAsync(ProjectFinancialAnalysis analysis);
    Task UpdateAsync(ProjectFinancialAnalysis analysis);
}
