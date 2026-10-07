namespace PropertyRental.API.PropertyManagement.Property;

public static class PropertyRoutes
{
    private const string Base = "api/manager/properties";
    private const string ById = $"/{{id:guid}}";

    public const string GetAll = Base;
    public const string GetById = $"{Base}{ById}";
    public const string Create = Base;
    public const string Update = $"{Base}{ById}";
    public const string Delete = $"{Base}{ById}";
}