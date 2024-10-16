namespace Wallet.Domain.Date;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}