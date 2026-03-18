using System.Text.RegularExpressions;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.AuthService.Domain.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public string Valor { get; }

    private Email(string valor)
    {
        Valor = valor;
    }

    public static Email Criar(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException("Email não pode ser vazio.");

        if (!EmailRegex.IsMatch(valor))
            throw new DomainException("Email inválido.");

        return new Email(valor.ToLowerInvariant());
    }

    public override string ToString()
        => Valor;

    public bool Equals(Email? other)
    {
        if (other is null)
            return false;

        return Valor == other.Valor;
    }

    public override bool Equals(object? obj)
        => obj is Email other && Equals(other);

    public override int GetHashCode()
        => Valor.GetHashCode();

    public static implicit operator string(Email email)
        => email.Valor;
}