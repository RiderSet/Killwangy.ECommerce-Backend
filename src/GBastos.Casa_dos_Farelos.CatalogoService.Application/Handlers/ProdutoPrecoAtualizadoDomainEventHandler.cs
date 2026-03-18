using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using GBastos.Casa_dos_Farelos.CatalogoService.Domain.Events;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CatalogoService.Application.Handlers;

public class ProdutoPrecoAtualizadoDomainEventHandler
    : INotificationHandler<ProdutoPrecoAtualizadoDomainEvent>
{
    public Task Handle(ProdutoPrecoAtualizadoDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}