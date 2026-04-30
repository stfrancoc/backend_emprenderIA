using Microsoft.EntityFrameworkCore;
using EmprendeIA.Domain.Projects;
using EmprendeIA.Domain.Entities;
using EmprendeIA.Domain.Profiles;

namespace EmprendeIA.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<User> Users => Set<User>();

    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<ProjectBmc> ProjectBmcs { get; set; }
    public DbSet<ProjectFinancialAnalysis> ProjectFinancialAnalyses { get; set; }
    public DbSet<ChatSession> ChatSessions { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<EntrepreneurProfile> EntrepreneurProfiles { get; set; }
    public DbSet<InvestorProfile> InvestorProfiles { get; set; }
    public DbSet<MentorProfile> MentorProfiles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Store enums as strings
        modelBuilder.Entity<Project>()
            .Property(p => p.Stage)
            .HasConversion<string>();

        modelBuilder.Entity<Project>()
            .Property(p => p.Status)
            .HasConversion<string>();

        // Configure User table
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).HasMaxLength(120);
            entity.Property(u => u.Email).HasMaxLength(255);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Role).HasMaxLength(50);
            entity.Property(u => u.IsActive).HasDefaultValue(true);
        });

        // Configure Financial Analysis
        modelBuilder.Entity<ProjectFinancialAnalysis>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.HasOne(f => f.Project)
                .WithOne(p => p.FinancialAnalysis)
                .HasForeignKey<ProjectFinancialAnalysis>(f => f.ProjectId);
        });

        // Configure Chat
        modelBuilder.Entity<ChatSession>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasMany(s => s.Messages)
                .WithOne()
                .HasForeignKey(m => m.ChatSessionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(m => m.Id);
        });
    }
}