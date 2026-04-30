using EmprendeIA.Domain.Entities;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmprendeIA.Infrastructure.Repositories;

public class FinancialRepository : IFinancialRepository
{
    private readonly ApplicationDbContext _context;

    public FinancialRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectFinancialAnalysis?> GetByProjectIdAsync(Guid projectId)
    {
        return await _context.ProjectFinancialAnalyses
            .FirstOrDefaultAsync(f => f.ProjectId == projectId);
    }

    public async Task AddAsync(ProjectFinancialAnalysis analysis)
    {
        await _context.ProjectFinancialAnalyses.AddAsync(analysis);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProjectFinancialAnalysis analysis)
    {
        _context.ProjectFinancialAnalyses.Update(analysis);
        await _context.SaveChangesAsync();
    }
}
