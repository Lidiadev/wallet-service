namespace Wallet.Domain;

public interface IUnitOfWorkFactory
{
    IUnitOfWork Create();
}