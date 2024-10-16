using System.ComponentModel.DataAnnotations;
using Wallet.Domain.Exceptions;
using Wallet.Domain.ValueObjects;

namespace Wallet.Domain.Entities;

public class Wallet
{
    private readonly List<Transaction> _transactions = [];
    
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Money Balance { get; private set; }
    
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    private Wallet()
    {
        Id = Guid.NewGuid();
        Balance = new Money(0);
    }

    private Wallet(Guid userId) : this()
    {
        UserId = userId;
    }

    public static Wallet Create(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("User ID cannot be empty", nameof(userId));
        }
        
        if (!Guid.TryParse(userId, out var user))
        {
            throw new ArgumentException("User ID is not valid", nameof(userId));
        }
        
        return new Wallet(user);
    }
    
    public static Wallet Create(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID is not valid", nameof(userId));
        }
        
        return new Wallet(userId);
    }

    public void AddFunds(Money amount, Guid providerId, string providerTransactionId, DateTime timestamp)
    {
        if (amount.Amount <= 0)
        {
            throw new InvalidOperationException("Amount must be positive");
        }

        Balance += amount;
        _transactions.Add(Transaction.Create(Id, providerId, providerTransactionId, amount, TransactionType.Deposit, timestamp));
    }

    public void RemoveFunds(Money amount, Guid providerId, string providerTransactionId, DateTime timestamp)
    {
        if (amount.Amount <= 0)
        {
            throw new InvalidOperationException("Amount must be positive");
        }

        if (Balance.Amount < amount.Amount)
        {
            throw new InsufficientFundsException(Id, Balance.Amount, amount.Amount);
        }

        Balance -= amount;
        _transactions.Add(Transaction.Create(Id, providerId, providerTransactionId, amount, TransactionType.Withdrawal, timestamp));
    }
    
    public void LoadTransactions(IEnumerable<Transaction> transactions)
    {
        _transactions.Clear();
        _transactions.AddRange(transactions);
    }
    
    public bool IsTransactionAlreadyProcessed(string providerTransactionId) 
        => _transactions.Any(t => t.ProviderTransactionId == providerTransactionId);
}