namespace Wallet.API.Models;

public class RemoveFundsModel
{
    public decimal Amount { get; set; }
    public Guid ProviderId { get; set; }
    public string TransactionId { get; set; }
}