namespace PropertyRental.API.PropertyManagement.Unit;

public static class UnitRoutes
{
    private const string Base = "api/units";

    public const string Create = "api/properties/{propertyId}/units";
    public const string Update = $"{Base}/{{id}}";
    public const string Delete = $"{Base}/{{id}}";
}