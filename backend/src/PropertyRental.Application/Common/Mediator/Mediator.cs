using Microsoft.Extensions.DependencyInjection;
using PropertyRental.Application.Common.Mediator.Abstractions;

namespace PropertyRental.Application.Common.Mediator;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task ExecuteQueryAsync<T>(
        T query,
        CancellationToken cancellationToken)
    {
        var queryHandler =
            serviceProvider.GetRequiredService<IQueryHandler<T>>();

        await queryHandler.ExecuteAsync(query, cancellationToken);
    }

    public async Task ExecuteCommandAsync<T>(
        T command,
        CancellationToken cancellationToken)
    {
        var commandHandler =
            serviceProvider.GetRequiredService<ICommandHandler<T>>();

        await commandHandler.ExecuteAsync(command, cancellationToken);
    }

    public async Task<TResult> ExecuteQueryAsync<T, TResult>(
        T query,
        CancellationToken cancellationToken)
    {
        var queryHandler =
            serviceProvider.GetRequiredService<IQueryHandler<T, TResult>>();

        return await queryHandler.ExecuteAsync(query, cancellationToken);
    }

    public async Task<TResult> ExecuteCommandAsync<T, TResult>(
        T command,
        CancellationToken cancellationToken)
    {
        var commandHandler =
            serviceProvider.GetRequiredService<ICommandHandler<T, TResult>>();

        return await commandHandler.ExecuteAsync(command, cancellationToken);
    }
}