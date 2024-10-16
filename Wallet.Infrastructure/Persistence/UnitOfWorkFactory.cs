using Wallet.Domain;

namespace Wallet.Infrastructure.Persistence;

public class UnitOfWorkFactory(WalletContext context) : IUnitOfWorkFactory
{
    public IUnitOfWork Create()
    {
        return new UnitOfWork(context);
    }
}