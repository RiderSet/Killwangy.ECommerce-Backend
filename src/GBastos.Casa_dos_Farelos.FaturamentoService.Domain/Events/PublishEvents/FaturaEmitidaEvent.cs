using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.Publish;

public record FaturaEmitidaEvent(
    Guid FaturaId,
    string Numero,
    decimal ValorTotal,
    DateTime DataEmissao
) : INotification;
