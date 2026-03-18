using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;

[Owned]
public sealed record PlacaVeiculo
{
    public string Valor { get; private init; } = null!;

    private PlacaVeiculo() { }

    public PlacaVeiculo(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException("Placa é obrigatória.");

        valor = valor.ToUpper().Trim();

        if (valor.Length < 6 || valor.Length > 8)
            throw new DomainException("Placa inválida.");

        if (!Regex.IsMatch(valor, "^[A-Z0-9-]+$"))
            throw new DomainException("Placa contém caracteres inválidos.");

        Valor = valor;
    }

    public override string ToString() => Valor;
}