namespace PropertyRental.Domain.Entities.PropertyEntities;

public class UnitType
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}