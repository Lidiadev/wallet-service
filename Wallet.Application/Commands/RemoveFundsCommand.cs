using MediatR;
using Wallet.Application.Interfaces;
using Wallet.Domain;
using Wallet.Domain.Date;
using Wallet.Domain.Exceptions;
using Wallet.Domain.ValueObjects;

namespace Wallet.Application.Commands;

public record RemoveFundsCommand(Guid WalletId, decimal Amount, Guid ProviderId, string ProviderTransactionId)
    : IRequest<Unit>;

public class RemoveFundsCommandHandler(
    IWalletRepository walletRepository,
    IUnitOfWorkFactory unitOfWorkFactory,
    ICacheService cacheService,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<RemoveFundsCommand, Unit>
{
    public async Task<Unit> Handle(RemoveFundsCommand request, CancellationToken cancellationToken)
    {
        await using var unitOfWork = unitOfWorkFactory.Create();
        
        await unitOfWork.ExecuteAsync(async () =>
        {
            var wallet = await walletRepository.GetByIdWithLockAsync(request.WalletId);

            if (wallet.IsTransactionAlreadyProcessed(request.ProviderTransactionId))
            {
                throw new DuplicateTransactionException(request.WalletId, request.ProviderTransactionId);
            }

            wallet.RemoveFunds(new Money(request.Amount), request.ProviderId, request.ProviderTransactionId, dateTimeProvider.UtcNow);

            await walletRepository.UpdateAsync(wallet);
        });
        
        await cacheService.RemoveAsync($"wallet:{request.WalletId}");

        return Unit.Value;
    }
}