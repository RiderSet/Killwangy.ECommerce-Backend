using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.PublishEvents;

public record FaturaVencidaEvent(Guid FaturaId, string Numero, DateTime DataVencimento) : INotification;