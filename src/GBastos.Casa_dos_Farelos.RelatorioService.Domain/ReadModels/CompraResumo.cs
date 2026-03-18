namespace GBastos.Casa_dos_Farelos.RelatorioService.Domain.ReadModels;

public class CompraResumo
{
    public Guid CompraId { get; set; }
    public Guid FornecedorId { get; set; }
    public Guid FuncionarioId { get; set; }
    public string NomeFuncionario { get; set; } = string.Empty;
    public DateTime DataCompra { get; set; }
    public decimal ValorTotal { get; set; }
    public List<CompraItemResumo> Itens { get; set; } = new();
}