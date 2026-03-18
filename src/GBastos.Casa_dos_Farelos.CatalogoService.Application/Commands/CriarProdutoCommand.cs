using MediatR;

namespace GBastos.Casa_dos_Farelos.CatalogoService.Application.Commands;

public record CriarProdutoCommand(
    string Nome,
    string Descricao,
    decimal Preco
) : IRequest<Guid>;