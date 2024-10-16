namespace Wallet.Domain;

public interface IWalletRepository
{
    Task<Entities.Wallet> GetByIdAsync(Guid id);
    Task<Entities.Wallet> GetByIdWithLockAsync(Guid id);
    Task<Entities.Wallet> CreateAsync(Entities.Wallet wallet);
    Task UpdateAsync(Entities.Wallet wallet);
}