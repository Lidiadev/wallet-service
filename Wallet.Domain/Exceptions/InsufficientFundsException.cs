namespace Wallet.Domain.Exceptions;

public class InsufficientFundsException(Guid walletId, decimal currentBalance, decimal requestedAmount)
    : Exception(
        $"Insufficient funds in wallet {walletId}. Current balance: {currentBalance}, Requested amount: {requestedAmount}")
{
    public Guid WalletId { get; } = walletId;
    public decimal CurrentBalance { get; } = currentBalance;
    public decimal RequestedAmount { get; } = requestedAmount;
}