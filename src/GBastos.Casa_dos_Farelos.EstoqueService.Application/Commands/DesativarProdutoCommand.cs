using MediatR;
using static MassTransit.ValidationResultExtensions;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;

public record DesativarProdutoCommand(Guid Id) : IRequest<Result>;