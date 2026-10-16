using PropertyRental.Application.RentalManagement.Models;
using PropertyRental.Application.RentalManagement.Models.RentalApplication;
using PropertyRental.Application.RentalManagement.Models.Residence;
using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Application.RentalManagement.Extensions;

public static class RentalApplicationExtensions
{
    public static RentalApplication ToEntity(this CreateRentalApplicationModel model, Guid userId)
        => new()
        {
            UnitId = model.UnitId,
            UserId = userId,
            FullName = model.FullName,
            Phone = model.Phone,
            Email = model.Email,
            CurrentAddress = model.CurrentAddress,
            ResidenceHistory = model.ResidenceHistory
                .Select(x => x.ToEntity())
                .ToList()
        };

    private static Residence ToEntity(this CreateResidenceModel model)
        => new()
        {
            Address = model.Address,
            LandlordName = model.LandlordName,
            LandlordPhone = model.LandlordPhone,
            MoveInDate = model.MoveInDate,
            MoveOutDate = model.MoveOutDate
        };

    public static RentalApplicationModel ToRentalApplicationModel(this RentalApplication entity)
        => new(
            entity.Id,
            entity.UnitId,
            entity.FullName,
            entity.Phone,
            entity.Email,
            entity.CurrentAddress,
            entity.Status,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.ResidenceHistory
                .Select(x => ToResidenceModel(x)).ToList(),
            entity.StatusHistory
                .Select(x => ToStatusHistoryModel(x)).ToList(),
            Convert.ToBase64String(entity.RowVersion));

    public static ApplicantRentalApplicationModel ToApplicantRentalApplicationModel(this RentalApplication entity)
        => new(
            entity.Id,
            entity.UnitId,
            entity.FullName,
            entity.Phone,
            entity.Email,
            entity.CurrentAddress,
            entity.Status,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.ResidenceHistory
                .Select(x => ToResidenceModel(x))
                .ToList());

    private static ResidenceModel ToResidenceModel(this Residence entity)
        => new(
            entity.Id,
            entity.Address,
            entity.LandlordName,
            entity.LandlordPhone,
            entity.MoveInDate,
            entity.MoveOutDate);

    private static ApplicationStatusHistoryModel ToStatusHistoryModel(this ApplicationStatusHistory entity)
        => new(
            entity.Id,
            entity.FromStatus,
            entity.ToStatus,
            entity.ChangedAt,
            entity.ChangedByUserId,
            entity.Comment);
}