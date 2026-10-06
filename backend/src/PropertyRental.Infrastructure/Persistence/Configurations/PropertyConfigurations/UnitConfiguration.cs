using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Infrastructure.Persistence.Configurations.PropertyConfigurations;

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UnitNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.MonthlyRent)
            .HasPrecision(18, 2);

        builder.HasOne(x => x.Property)
            .WithMany(x => x.Units)
            .HasForeignKey(x => x.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.UnitType)
            .WithMany()
            .HasForeignKey(x => x.UnitTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}