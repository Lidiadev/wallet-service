namespace Wallet.Domain.Exceptions;

public class DuplicateTransactionException(Guid walletId, string transactionId)
    : Exception($"A transaction {transactionId} for wallet {walletId} already exists.")
{
    public Guid WalletId { get; } = walletId;
    public string TransactionId { get; } = transactionId;
}