using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Infrastructure.Persistence.Configurations.RentalConfigurations;

public class LeaseConfiguration : IEntityTypeConfiguration<Lease>
{
    public void Configure(EntityTypeBuilder<Lease> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.MonthlyRent)
            .HasPrecision(18, 2);

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.EndDate)
            .IsRequired();

        builder.HasOne(x => x.Unit)
            .WithMany(x => x.Leases)
            .HasForeignKey(x => x.UnitId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}