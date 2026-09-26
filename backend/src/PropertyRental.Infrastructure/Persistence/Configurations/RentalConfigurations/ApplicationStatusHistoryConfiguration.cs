using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Infrastructure.Persistence.Configurations.RentalConfigurations;

public class ApplicationStatusHistoryConfiguration : IEntityTypeConfiguration<ApplicationStatusHistory>
{
    public void Configure(EntityTypeBuilder<ApplicationStatusHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ToStatus)
            .IsRequired();

        builder.Property(x => x.Comment)
            .HasMaxLength(1000);

        builder.HasOne(x => x.RentalApplication)
            .WithMany()
            .HasForeignKey(x => x.RentalApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ChangedByUser)
            .WithMany()
            .HasForeignKey(x => x.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}