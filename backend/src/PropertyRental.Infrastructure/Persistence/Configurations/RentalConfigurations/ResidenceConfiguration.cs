using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Infrastructure.Persistence.Configurations.RentalConfigurations;

public class ResidenceConfiguration : IEntityTypeConfiguration<Residence>
{
    public void Configure(EntityTypeBuilder<Residence> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.LandlordName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.LandlordPhone)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.MoveInDate)
            .IsRequired();

        builder.HasOne(x => x.RentalApplication)
            .WithMany(x => x.ResidenceHistory)
            .HasForeignKey(x => x.RentalApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}