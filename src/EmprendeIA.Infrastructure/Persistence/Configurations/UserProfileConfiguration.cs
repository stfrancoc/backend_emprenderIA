using EmprendeIA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmprendeIA.Infrastructure.Persistence.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(x => x.UserId);

        builder.HasOne(x => x.User)
            .WithOne(u => u.UserProfile)
            .HasForeignKey<UserProfile>(x => x.UserId);

        // PostgreSQL text[] for arrays
        builder.Property(x => x.Skills)
            .HasColumnType("text[]");

        builder.Property(x => x.Interests)
            .HasColumnType("text[]");

        builder.Property(x => x.Industries)
            .HasColumnType("text[]");

        builder.Property(x => x.ExperienceLevel)
            .HasMaxLength(20)
            .HasDefaultValue("junior");
    }
}
