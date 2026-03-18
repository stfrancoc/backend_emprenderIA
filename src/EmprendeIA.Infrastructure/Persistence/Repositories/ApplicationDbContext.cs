using Microsoft.EntityFrameworkCore;
using EmprendeIA.Domain.Projects;
using EmprendeIA.Domain.Entities;
using EmprendeIA.Domain.Profiles;

namespace EmprendeIA.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<User> Users => Set<User>();

    public DbSet<EntrepreneurProfile> EntrepreneurProfiles { get; set; }
    public DbSet<InvestorProfile> InvestorProfiles { get; set; }
    public DbSet<MentorProfile> MentorProfiles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}