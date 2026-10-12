using PropertyRental.Domain.Entities.Enums;

namespace PropertyRental.Application.RentalManagement.Models;

public record ApplicationStatusHistoryModel(
    Guid Id,
    ApplicationStatus? FromStatus,
    ApplicationStatus ToStatus,
    DateTimeOffset ChangedAt,
    Guid ChangedByUserId,
    string? Comment);