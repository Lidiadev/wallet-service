namespace Wallet.Domain.ValueObjects;

public class Money : IEquatable<Money>
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
        }

        Amount = amount;
    }

    public static Money operator +(Money left, Money right) => 
        new(left.Amount + right.Amount);

    public static Money operator -(Money left, Money right) => 
        new(left.Amount - right.Amount);

    public bool Equals(Money? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        
        return Amount == other.Amount;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        
        return Equals((Money)obj);
    }

    public override int GetHashCode() => 
        Amount.GetHashCode();
}