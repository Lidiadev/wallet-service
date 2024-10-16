using MediatR;
using Wallet.Application.Interfaces;
using Wallet.Application.Queries.Dtos;
using Wallet.Domain;
using Wallet.Domain.Exceptions;

namespace Wallet.Application.Queries;

public record GetWalletBalanceQuery(Guid WalletId) : IRequest<WalletDto>;

public class GetWalletBalanceQueryHandler(IWalletRepository walletRepository, ICacheService cacheService)
    : IRequestHandler<GetWalletBalanceQuery, WalletDto>
{
    public async Task<WalletDto> Handle(GetWalletBalanceQuery request, CancellationToken cancellationToken)
    {
        var wallet = await cacheService.GetOrSetAsync($"wallet:{request.WalletId}",
            async () => await walletRepository.GetByIdAsync(request.WalletId),
            TimeSpan.FromMinutes(5)); // Should be configured in appsettings

        if (wallet == null)
        {
            throw new NotFoundException(nameof(Wallet), request.WalletId);
        }

        var walletDto = new WalletDto
        {
            Id = wallet.Id,
            UserId = wallet.UserId,
            Balance = wallet.Balance.Amount,
            Transactions = wallet.Transactions
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount.Amount,
                    Type = t.Type.ToString(),
                    CreatedAt = t.Timestamp
                })
                .ToList()
        };
        
        return walletDto;
    }
}