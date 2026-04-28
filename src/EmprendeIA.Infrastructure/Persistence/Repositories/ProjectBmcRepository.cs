using EmprendeIA.Domain.Entities;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmprendeIA.Infrastructure.Repositories;

public class ProjectBmcRepository : IProjectBmcRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectBmcRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectBmc?> GetByProjectIdAsync(Guid projectId)
    {
        return await _context.ProjectBmcs
            .FirstOrDefaultAsync(b => b.ProjectId == projectId);
    }

    public async Task AddAsync(ProjectBmc bmc)
    {
        await _context.ProjectBmcs.AddAsync(bmc);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProjectBmc bmc)
    {
        _context.ProjectBmcs.Update(bmc);
        await _context.SaveChangesAsync();
    }
}
