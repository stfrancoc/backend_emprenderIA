using EmprendeIA.Domain.Entities;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmprendeIA.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.EntrepreneurProfile)
            .Include(u => u.UserProfile)
            .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}