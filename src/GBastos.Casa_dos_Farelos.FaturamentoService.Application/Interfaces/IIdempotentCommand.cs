using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Interfaces;

public interface IIdempotentCommand<TResponse> : IRequest<TResponse>
{
    string IdempotencyKey { get; }
}