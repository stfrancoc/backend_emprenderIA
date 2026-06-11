using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Projects;
using EmprendeIA.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmprendeIA.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;
    public ProjectRepository(ApplicationDbContext context) => _context = context;

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid id) 
        => await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId)
        => await _context.Projects.Where(p => p.OwnerId == ownerId).ToListAsync();

    public async Task<IEnumerable<Project>> GetByMinimumStageAsync(ProjectStage minStage)
    {
        // To avoid issues with enum-to-string conversions in EF Core queries,
        // fetch and filter in memory. If dataset is large, replace with a translated query.
        var all = await _context.Projects.ToListAsync();
        return all.Where(p => p.Stage >= minStage).ToList();
    }

    public async Task UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Project project)
    {
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }
}