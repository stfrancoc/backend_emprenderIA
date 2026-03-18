using EmprendeIA.Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class InvestorProfileConfiguration : IEntityTypeConfiguration<InvestorProfile>
{
    public void Configure(EntityTypeBuilder<InvestorProfile> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithOne(u => u.InvestorProfile)
            .HasForeignKey<InvestorProfile>(x => x.UserId);
    }
}