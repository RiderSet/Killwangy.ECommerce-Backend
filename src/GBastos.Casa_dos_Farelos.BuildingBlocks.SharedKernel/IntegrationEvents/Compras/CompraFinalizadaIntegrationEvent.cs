using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.Compras.DTOs;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.Compras;

public class CompraFinalizadaIntegrationEvent
{
    public Guid CompraId { get; set; }
    public Guid FornecedorId { get; set; }
    public Guid FuncionarioId { get; set; }
    public string NomeFuncionario { get; set; } = string.Empty;
    public DateTime DataCompra { get; set; }
    public decimal ValorTotal { get; set; }
    public List<CompraItemDto> Itens { get; set; } = new();
}