namespace PropertyRental.API.PropertyManagement.Unit;

public static class UnitRoutes
{
    private const string ById = $"/{{id:guid}}";

    public static class PropertyManager
    {
        private const string BaseManager = "api/manager/units";

        public const string Create = "api/manager/properties/{propertyId:guid}/units";
        public const string Update = $"{BaseManager}{ById}";
        public const string Delete = $"{BaseManager}{ById}";
    }

    public static class Applicant
    {
        private const string BaseApplicant = "api/applicant/units";

        public const string GetAvailable = BaseApplicant;
    }
}