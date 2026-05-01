using Microsoft.EntityFrameworkCore;
using EmprendeIA.Domain.Projects;
using EmprendeIA.Domain.Entities;
using EmprendeIA.Domain.Profiles;
using EmprendeIA.Domain.Entities.Marketplace;

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
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductMetrics> ProductMetrics { get; set; }

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

        // Configure BMC
        modelBuilder.Entity<ProjectBmc>(entity =>
        {
            entity.HasKey(b => b.ProjectId);
            entity.HasOne(b => b.Project)
                .WithOne(p => p.Bmc)
                .HasForeignKey<ProjectBmc>(b => b.ProjectId);
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

        // Configure Marketplace
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).HasMaxLength(120).IsRequired();
            entity.Property(p => p.Category).HasConversion<string>();
            entity.Property(p => p.Price).HasPrecision(18, 2);
            entity.Property(p => p.Visibility).HasDefaultValue(true);
            
            // PostgreSQL text[] mapping is usually automatic with List<string> in Npgsql 
            // but we can be explicit if needed.
            entity.Property(p => p.Images)
                .HasColumnType("text[]");

            entity.HasOne(p => p.Project)
                .WithMany()
                .HasForeignKey(p => p.ProjectId);

            entity.HasOne(p => p.Metrics)
                .WithOne(m => m.Product)
                .HasForeignKey<ProductMetrics>(m => m.ProductId);
        });

        modelBuilder.Entity<ProductMetrics>(entity =>
        {
            entity.HasKey(m => m.ProductId);
        });
    }
}