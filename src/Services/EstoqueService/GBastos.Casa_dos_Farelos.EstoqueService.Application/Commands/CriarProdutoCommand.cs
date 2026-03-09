using GBastos.Casa_dos_Farelos.SharedKernel.Common;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;

public record CriarProdutoCommand(
    string Nome,
    decimal Preco,
    int EstoqueInicial) : IRequest<Result<Guid>>;
