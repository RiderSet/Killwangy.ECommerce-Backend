namespace GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.PublishEvents;

public record FaturaCriadaEvent(Guid FaturaId, string Numero, Guid ClienteId, decimal ValorTotal);