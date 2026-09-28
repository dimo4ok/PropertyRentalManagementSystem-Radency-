namespace PropertyRental.Application.Common.Mediator.Abstractions;

public interface ICommandHandler<in T>
{
    Task ExecuteAsync(T command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in T, TResult>
{
    Task<TResult> ExecuteAsync(T command, CancellationToken cancellationToken);
}