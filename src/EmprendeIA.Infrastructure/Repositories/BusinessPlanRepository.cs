using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Projects;
using EmprendeIA.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmprendeIA.Infrastructure.Repositories;

public class BusinessPlanRepository : IBusinessPlanRepository
{
    private readonly ApplicationDbContext _context;

    public BusinessPlanRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BusinessPlan?> GetByProjectIdAsync(Guid projectId)
        => await _context.BusinessPlans.FirstOrDefaultAsync(b => b.ProjectId == projectId);

    public async Task AddAsync(BusinessPlan plan)
    {
        await _context.BusinessPlans.AddAsync(plan);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BusinessPlan plan)
    {
        _context.BusinessPlans.Update(plan);
        await _context.SaveChangesAsync();
    }
}
