namespace PropertyRental.Application.Common.Mediator.Abstractions;

public interface IMediator
{
    Task ExecuteQueryAsync<T>(T query, CancellationToken cancellationToken);
    Task ExecuteCommandAsync<T>(T command, CancellationToken cancellationToken);

    Task<TResult> ExecuteQueryAsync<T, TResult>(T query, CancellationToken cancellationToken);
    Task<TResult> ExecuteCommandAsync<T, TResult>(T command, CancellationToken cancellationToken);
}