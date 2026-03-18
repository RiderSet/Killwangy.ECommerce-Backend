using GBastos.Casa_dos_Farelos.RelatorioService.Domain.Common;

namespace GBastos.Casa_dos_Farelos.RelatorioService.Domain.ReadModels;

public class Veiculo : BaseEntity
{
    public string Placa { get; private set; } = string.Empty;
    public string Modelo { get; private set; } = string.Empty;
}