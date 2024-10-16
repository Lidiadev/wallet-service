using Wallet.Domain.Date;

namespace Wallet.Infrastructure.Date;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}