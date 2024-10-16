using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Wallet.Domain;
using Wallet.Domain.Entities;
using Wallet.Domain.Exceptions;
using Wallet.Domain.ValueObjects;
using Wallet.Infrastructure.Persistence;
using Wallet.Infrastructure.Repositories.Models;

namespace Wallet.Infrastructure.Repositories;

public class WalletRepository(WalletContext context) : IWalletRepository
{
    public async Task<Domain.Entities.Wallet> GetByIdAsync(Guid id)
    {
        var walletModel = await context.Wallets
            .Include(w => w.Transactions)
            .FirstOrDefaultAsync(w => w.Id == id);
        
        if (walletModel == null)
        {
            throw new NotFoundException(nameof(Wallet), id);
        }
        
        return MapToEntity(walletModel);
    }

    public async Task<Domain.Entities.Wallet> GetByIdWithLockAsync(Guid id)
    {
        var walletModel = await context.Wallets
            .FromSqlRaw("SELECT * FROM Wallets WITH (UPDLOCK, ROWLOCK) WHERE Id = {0}", id)
            .AsNoTracking()
            .Include(w => w.Transactions)
            .FirstOrDefaultAsync();

        if (walletModel == null)
        {
            throw new NotFoundException(nameof(Wallet), id);
        }

        return MapToEntity(walletModel);
    }

    public async Task<Domain.Entities.Wallet> CreateAsync(Domain.Entities.Wallet wallet)
    {
        var walletModel = MapToModel(wallet);
        
        try
        {
            await context.Wallets.AddAsync(walletModel);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (IsDuplicateKeyViolation(ex))
            {
                throw new DuplicateWalletException($"A wallet for user {wallet.UserId} already exists.");
            }
            throw;
        }
        
        return MapToEntity(walletModel);
    }

    public async Task UpdateAsync(Domain.Entities.Wallet wallet)
    {
        var walletModel = MapToModel(wallet);
        
        context.Wallets.Attach(walletModel);
        context.Entry(walletModel).State = EntityState.Modified;
        await context.Transactions.AddAsync(walletModel.Transactions.Last());
    }

    private static Domain.Entities.Wallet MapToEntity(WalletModel model)
    {
        var wallet = Domain.Entities.Wallet.Create(model.UserId);
        var walletType = typeof(Domain.Entities.Wallet);
        walletType.GetProperty("Id")?.SetValue(wallet, model.Id);
        walletType.GetProperty("Balance")?.SetValue(wallet, new Money(model.Balance));

        var transactions = model
            .Transactions
            .Select(t => Transaction.Create(
                t.WalletId,
                t.ProviderId,
                t.ProviderTransactionId,
                new Money(t.Amount),
                Enum.Parse<TransactionType>(t.Type),
                t.Timestamp
            ));

        wallet.LoadTransactions(transactions);

        return wallet;
    }

    private static WalletModel MapToModel(Domain.Entities.Wallet wallet) =>
        new()
        {
            Id = wallet.Id,
            UserId = wallet.UserId,
            Balance = wallet.Balance.Amount,
            Transactions = wallet.Transactions
                .Select(t => new TransactionModel
                {
                    Id = t.Id,
                    WalletId = t.WalletId,
                    ProviderId = t.ProviderId,
                    ProviderTransactionId = t.ProviderTransactionId,
                    Amount = t.Amount.Amount,
                    Type = t.Type.ToString(),
                    Timestamp = t.Timestamp
                })
                .ToList()
        };

    private static bool IsDuplicateKeyViolation(DbUpdateException ex) => 
        ex.InnerException is SqlException { Number: 2627 or 2601 };
}