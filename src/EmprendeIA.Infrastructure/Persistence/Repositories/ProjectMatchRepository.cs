using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Projects;
using Microsoft.EntityFrameworkCore;

namespace EmprendeIA.Infrastructure.Persistence.Repositories;

public class ProjectMatchRepository : IProjectMatchRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectMatchRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectMatch?> GetByIdAsync(Guid id)
    {
        return await _context.ProjectMatches.FindAsync(id);
    }

    public async Task<ProjectMatch?> GetMatchAsync(Guid projectId, Guid investorId)
    {
        return await _context.ProjectMatches
            .FirstOrDefaultAsync(m => m.ProjectId == projectId && m.InvestorId == investorId);
    }

    public async Task<List<ProjectMatch>> GetMatchesByUserAsync(Guid userId)
    {
        return await _context.ProjectMatches
            .Include(m => m.Project)
            .Where(m => m.Project.OwnerId == userId || m.InvestorId == userId)
            .OrderByDescending(m => m.CreatedAt)
            .Take(10)
            .ToListAsync();
    }

    public async Task<List<ProjectMatch>> GetMatchesByProjectAsync(Guid projectId)
    {
        return await _context.ProjectMatches
            .Include(m => m.Project)
            .Where(m => m.ProjectId == projectId)
            .OrderByDescending(m => m.MatchScore)
            .ToListAsync();
    }

    public async Task AddAsync(ProjectMatch match)
    {
        await _context.ProjectMatches.AddAsync(match);
        await _context.SaveChangesAsync();
    }
}
