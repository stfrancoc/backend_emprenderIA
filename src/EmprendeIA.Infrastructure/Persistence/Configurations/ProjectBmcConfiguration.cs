using EmprendeIA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmprendeIA.Infrastructure.Persistence.Configurations;

public class ProjectBmcConfiguration : IEntityTypeConfiguration<ProjectBmc>
{
    public void Configure(EntityTypeBuilder<ProjectBmc> builder)
    {
        builder.HasKey(x => x.ProjectId);

        builder.HasOne(x => x.Project)
            .WithOne(p => p.Bmc)
            .HasForeignKey<ProjectBmc>(x => x.ProjectId);
    }
}
