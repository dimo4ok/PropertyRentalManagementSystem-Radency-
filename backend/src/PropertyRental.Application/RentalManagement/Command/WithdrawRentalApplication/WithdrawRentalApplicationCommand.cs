using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;

namespace PropertyRental.Application.RentalManagement.Command.WithdrawRentalApplication;

public record WithdrawRentalApplicationCommand(Guid Id);