using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Infrastructure.Persistence.Configurations.PropertyConfigurations;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasMany(x => x.Units)
            .WithOne(x => x.Property)
            .HasForeignKey(x => x.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.UpdatedAt)
            .IsRequired();
        
        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}