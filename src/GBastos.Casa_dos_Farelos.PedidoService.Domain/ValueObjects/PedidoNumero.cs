using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Domain.ValueObjects;

public class PedidoNumero
{
    public int Valor { get; }

    public PedidoNumero(int valor)
    {
        if (valor <= 0)
            throw new DomainException("Número do pedido inválido. Deve ser maior que zero.");

        Valor = valor;
    }

    // Sobrescrevendo Equals para Value Object
    public override bool Equals(object? obj)
    {
        if (obj is PedidoNumero outro)
            return Valor == outro.Valor;

        return false;
    }

    public override int GetHashCode() => Valor.GetHashCode();
    public override string ToString() => Valor.ToString();
}
