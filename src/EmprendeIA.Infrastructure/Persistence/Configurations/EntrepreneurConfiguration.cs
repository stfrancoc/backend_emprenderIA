using EmprendeIA.Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EntrepreneurProfileConfiguration : IEntityTypeConfiguration<EntrepreneurProfile>
{
    public void Configure(EntityTypeBuilder<EntrepreneurProfile> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithOne(u => u.EntrepreneurProfile)
            .HasForeignKey<EntrepreneurProfile>(x => x.UserId);
    }
}