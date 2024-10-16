using Wallet.Domain.ValueObjects;

namespace Wallet.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid WalletId { get; private set; }
    public Guid ProviderId { get; private set; }
    public string ProviderTransactionId { get; private set; }
    public Money Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public DateTime Timestamp { get; private set; }

    private Transaction()
    {
        Id = Guid.NewGuid();
    }

    private Transaction(
        Guid walletId,
        Guid providerId,
        string providerTransactionId,
        Money amount,
        TransactionType type,
        DateTime timestamp)
        : this()
    {
        WalletId = walletId;
        ProviderId = providerId;
        ProviderTransactionId = providerTransactionId;
        Amount = amount;
        Type = type;
        Timestamp = timestamp;
    }

    public static Transaction Create(
        Guid walletId,
        Guid providerId,
        string providerTransactionId,
        Money amount,
        TransactionType type,
        DateTime timestamp)
    {
        if (walletId == Guid.Empty)
        {
            throw new ArgumentException("WalletId cannot be empty", nameof(walletId));
        }
        if (providerId == Guid.Empty)
        {
            throw new ArgumentException("ProviderId cannot be empty", nameof(providerId));
        }
        if (string.IsNullOrWhiteSpace(providerTransactionId))
        {
            throw new ArgumentException("ProviderTransactionId cannot be null or empty", nameof(providerTransactionId));
        }
        if (amount.Amount <= 0)
        {
            throw new ArgumentException("Amount must be positive", nameof(amount));
        }

        return new Transaction(walletId, providerId, providerTransactionId, amount, type, timestamp);
    }
}