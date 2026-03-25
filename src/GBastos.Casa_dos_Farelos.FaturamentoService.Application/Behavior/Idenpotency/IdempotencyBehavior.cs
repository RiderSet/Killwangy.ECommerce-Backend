using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Behavior.Idenpotency;

public class IdempotencyBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IIdempotentCommand<TResponse>
{
    private readonly IIdempotencyStore _store;

    public IdempotencyBehavior(IIdempotencyStore store)
    {
        _store = store;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var key = request.IdempotencyKey;

        if (await _store.ExistsAsync(key))
        {
            return default!;
        }

        var response = await next();

        await _store.SetAsync(key);

        return response;
    }
}