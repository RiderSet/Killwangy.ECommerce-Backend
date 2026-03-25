using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.Publish;

public record FaturaCanceladaEvent(
    Guid FaturaId,
    string Numero,
    DateTime DataCancelamento
) : INotification;