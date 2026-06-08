using EmprendeIA.Domain.Projects;

namespace EmprendeIA.Domain.Interfaces;

public interface IBusinessPlanRepository
{
    Task<BusinessPlan?> GetByProjectIdAsync(Guid projectId);
    Task AddAsync(BusinessPlan plan);
    Task UpdateAsync(BusinessPlan plan);
}
