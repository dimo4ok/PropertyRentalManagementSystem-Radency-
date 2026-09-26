namespace PropertyRental.Domain.Entities.PropertyEntities;

public class Property
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;

    public ICollection<Unit> Units { get; set; } = [];
}