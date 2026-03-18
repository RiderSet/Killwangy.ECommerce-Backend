using GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;

public class VeiculoDto
{
    public Guid Id { get; set; }
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Ano { get; set; }
    public string Cor { get; set; } = string.Empty;
    public PlacaVeiculo? Placa { get; set; }
    public TipoVeiculo? Tipo { get; set; }
    public decimal Preco { get; set; }

    public bool Disponivel { get; set; } = true;
}