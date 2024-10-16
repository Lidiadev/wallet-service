using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Wallet.Domain;

namespace Wallet.Infrastructure.Persistence;

public class UnitOfWork(WalletContext context) : IUnitOfWork
{
    private IDbContextTransaction _transaction;

    public async Task ExecuteAsync(Func<Task> action)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                await action();
                
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }
        
        await context.DisposeAsync();
    }
}