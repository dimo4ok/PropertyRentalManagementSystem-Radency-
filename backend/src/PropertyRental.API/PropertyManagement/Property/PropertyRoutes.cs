namespace PropertyRental.API.PropertyManagement.Property;

public static class PropertyRoutes
{
    private const string Base = "api/properties";

    public const string GetAll = Base;
    public const string GetById = $"{Base}/{{id}}";
    public const string Create = Base;
    public const string Update = $"{Base}/{{id}}";
    public const string Delete = $"{Base}/{{id}}";
}