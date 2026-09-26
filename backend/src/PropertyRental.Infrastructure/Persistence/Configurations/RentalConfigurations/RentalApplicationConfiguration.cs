using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Infrastructure.Persistence.Configurations.RentalConfigurations;

public class RentalApplicationConfiguration : IEntityTypeConfiguration<RentalApplication>
{
    public void Configure(EntityTypeBuilder<RentalApplication> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(320);

        builder.Property(x => x.CurrentAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(x => x.Unit)
            .WithMany()
            .HasForeignKey(x => x.UnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Applications)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ResidenceHistory)
            .WithOne(x => x.RentalApplication)
            .HasForeignKey(x => x.RentalApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}