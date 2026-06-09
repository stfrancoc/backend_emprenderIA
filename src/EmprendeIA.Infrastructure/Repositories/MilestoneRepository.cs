using EmprendeIA.Domain.Entities;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmprendeIA.Infrastructure.Repositories;

public class MilestoneRepository : IMilestoneRepository
{
    private readonly ApplicationDbContext _context;

    public MilestoneRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Milestone?> GetByIdAsync(Guid id)
        => await _context.Milestones.FirstOrDefaultAsync(m => m.Id == id);

    public async Task<IEnumerable<Milestone>> GetByProjectIdAsync(Guid projectId)
        => await _context.Milestones.Where(m => m.ProjectId == projectId).ToListAsync();

    public async Task AddAsync(Milestone milestone)
    {
        await _context.Milestones.AddAsync(milestone);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Milestone milestone)
    {
        _context.Milestones.Update(milestone);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var milestone = await GetByIdAsync(id);
        if (milestone != null)
        {
            _context.Milestones.Remove(milestone);
            await _context.SaveChangesAsync();
        }
    }
}
