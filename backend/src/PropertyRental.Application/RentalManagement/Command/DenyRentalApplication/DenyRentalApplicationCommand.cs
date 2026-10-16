using PropertyRental.Application.RentalManagement.Models.RentalApplication;

namespace PropertyRental.Application.RentalManagement.Command.DenyRentalApplication;

public record DenyRentalApplicationCommand(Guid Id, RentalApplicationReviewModel Model);
