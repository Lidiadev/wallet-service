using MediatR;
using Wallet.Domain;
using Wallet.Domain.Exceptions;

namespace Wallet.Application.Commands;

public record CreateWalletCommand(string UserId) : IRequest<Guid>;

public class CreateWalletCommandHandler(IWalletRepository walletRepository) : IRequestHandler<CreateWalletCommand, Guid>
{
    public async Task<Guid> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = Domain.Entities.Wallet.Create(request.UserId);
        
        try
        {
            var createdWallet = await walletRepository.CreateAsync(wallet);
            
            return createdWallet.Id;
        }
        catch (DuplicateWalletException ex) 
        {
            throw;
        }
    }
}