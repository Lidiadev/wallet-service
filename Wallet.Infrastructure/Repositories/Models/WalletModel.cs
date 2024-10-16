using System.ComponentModel.DataAnnotations;

namespace Wallet.Infrastructure.Repositories.Models;

public class WalletModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
    public List<TransactionModel> Transactions { get; set; } = [];
}