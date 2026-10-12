namespace PropertyRental.API.RentalManagement.RentalApplication;

public static class RentalApplicationRoutes
{
    private const string ById = $"/{{id:guid}}";

    public static class Applicant
    {
        private const string BaseApplicant = "api/applicant/rental-applications";

        public const string Create = BaseApplicant;
        public const string GetAll = BaseApplicant;
        public const string GetById = $"{BaseApplicant}{ById}";
        public const string Update = $"{BaseApplicant}{ById}";
        public const string Submit = $"{BaseApplicant}{ById}/submit";
        public const string Withdraw = $"{BaseApplicant}{ById}/withdraw";
    }

    public static class PropertyManager
    {
        private const string BaseManager = "api/manager/rental-applications";

        public const string GetAll = BaseManager;
        public const string GetById = $"{BaseManager}{ById}";
        public const string Approve = $"{BaseManager}{ById}/approve";
        public const string Return = $"{BaseManager}{ById}/return";
        public const string Deny = $"{BaseManager}{ById}/deny";
    }
}