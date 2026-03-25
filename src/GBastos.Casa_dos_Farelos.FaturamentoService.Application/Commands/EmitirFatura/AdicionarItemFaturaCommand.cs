using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Commands.EmitirFatura;

public record AdicionarItemFaturaCommand(
    Guid FaturaId,
    Guid ProdutoId,
    int Quantidade,
    decimal ValorUnitario
) : IRequest<Unit>;