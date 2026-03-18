using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GBastos.Casa_dos_Farelos.CadastroService.Api.Contracts;

public sealed class CriarVeiculoRequest
{
    [Required]
    [StringLength(10, MinimumLength = 1)]
    public string Placa { get; set; } = null!;

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Modelo { get; set; } = null!;

    [Required]
    public Guid ClienteId { get; set; }

    [Required]
    public TipoVeiculo TipoVeiculo { get; set; } = TipoVeiculo.ProprioEmpresa;
}