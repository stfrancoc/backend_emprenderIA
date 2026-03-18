using EmprendeIA.Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MentorProfileConfiguration : IEntityTypeConfiguration<MentorProfile>
{
    public void Configure(EntityTypeBuilder<MentorProfile> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithOne(u => u.MentorProfile)
            .HasForeignKey<MentorProfile>(x => x.UserId);
    }
}