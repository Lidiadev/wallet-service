using Wallet.Application.Queries.Dtos;

namespace Wallet.API.Models;

public class GetWalletTransactionModel(TransactionDto transaction)
{
    public Guid Id { get;  set; } = transaction.Id;
    public decimal Amount { get; set; } = transaction.Amount;
    public string Type { get; set; } = transaction.Type;
    public DateTime CreatedAt { get; set; } = transaction.CreatedAt;
}