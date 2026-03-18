using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;

public sealed class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = null!;
    private Money() { }

    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new DomainException("Money amount cannot be negative.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency is required.");

        Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
        Currency = currency.ToUpperInvariant();
    }

    public static Money Zero(string currency)
        => new(0m, currency);

    public static Money operator +(Money a, Money b)
    {
        EnsureSameCurrency(a, b);

        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money operator -(Money a, Money b)
    {
        EnsureSameCurrency(a, b);

        if (a.Amount < b.Amount)
            throw new DomainException("Money result cannot be negative.");

        return new Money(a.Amount - b.Amount, a.Currency);
    }

    public static Money operator *(Money money, int multiplier)
    {
        if (multiplier < 0)
            throw new DomainException("Multiplier cannot be negative.");

        return new Money(money.Amount * multiplier, money.Currency);
    }

    public static bool operator >(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Amount > b.Amount;
    }

    public static bool operator <(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Amount < b.Amount;
    }

    public static implicit operator Money(int v)
    {
        throw new NotImplementedException();
    }

    private static void EnsureSameCurrency(Money a, Money b)
    {
        if (!string.Equals(a.Currency, b.Currency, StringComparison.Ordinal))
            throw new DomainException("Currency mismatch.");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString()
        => $"{Currency} {Amount:n2}";
}