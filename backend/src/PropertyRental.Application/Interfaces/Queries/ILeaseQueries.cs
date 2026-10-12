namespace PropertyRental.Application.Interfaces.Queries;

public interface ILeaseQueries
{
    Task<bool> HasActiveLeaseAsync(Guid unitId, CancellationToken cancellationToken);
}