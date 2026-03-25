using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.PublishEvents;

public record FaturaPagaEvent(Guid FaturaId, string Numero, DateTime DataPagamento) : INotification;