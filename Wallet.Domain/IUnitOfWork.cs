namespace Wallet.Domain;

public interface IUnitOfWork
{
    Task ExecuteAsync(Func<Task> action);
     ValueTask DisposeAsync();
}