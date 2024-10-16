using Wallet.Application.Queries.Dtos;

namespace Wallet.API.Models;

public class GetWalletResponseModel
{
    public Guid WalletId { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
    public ICollection<GetWalletTransactionModel> Transactions { get; set; }

    public GetWalletResponseModel(WalletDto wallet)
    {
        WalletId = wallet.Id;
        UserId = wallet.UserId;
        Balance = wallet.Balance;
        Transactions = new List<GetWalletTransactionModel>();

        foreach (var walletTransaction in wallet.Transactions)
        {
            Transactions.Add(new GetWalletTransactionModel(walletTransaction));
        }
    }
}