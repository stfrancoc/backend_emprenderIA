using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Projects;
using EmprendeIA.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmprendeIA.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
    }
}