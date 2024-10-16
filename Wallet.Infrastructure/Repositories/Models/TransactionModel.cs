namespace Wallet.Infrastructure.Repositories.Models;

public class TransactionModel
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public Guid ProviderId { get; set; }
    public string ProviderTransactionId { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public DateTime Timestamp { get; set; }
    public WalletModel Wallet { get; set; }
}