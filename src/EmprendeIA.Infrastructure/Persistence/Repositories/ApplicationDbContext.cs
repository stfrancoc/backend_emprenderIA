using Microsoft.EntityFrameworkCore;
using EmprendeIA.Domain.Projects;
using EmprendeIA.Domain.Entities;

namespace EmprendeIA.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<User> Users => Set<User>();
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}