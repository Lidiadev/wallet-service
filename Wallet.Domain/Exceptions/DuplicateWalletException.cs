namespace Wallet.Domain.Exceptions;

public class DuplicateWalletException(string message) : Exception(message);