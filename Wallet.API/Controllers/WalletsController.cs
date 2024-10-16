using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models;
using Wallet.Application.Commands;
using Wallet.Application.Queries;

namespace Wallet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<WalletsController> _logger;

    public WalletsController(IMediator mediator, ILogger<WalletsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateWallet(CreateWalletModel model)
    {
        var walletId = await _mediator.Send(new CreateWalletCommand(model.UserId));
        
        _logger.LogInformation("Wallet created with ID: {WalletId}", walletId);

        var response = new CreateWalletResponseModel { Id = walletId };
        
        return CreatedAtAction(nameof(GetWallet), response, walletId);
    }

    [HttpPost("{id:guid}/add-funds")]
    public async Task<IActionResult> AddFunds(Guid id, [FromBody] AddFundsModel model)
    {
        await _mediator.Send(new AddFundsCommand(id, model.Amount, model.ProviderId, model.TransactionId));
        _logger.LogInformation("Funds added to wallet {WalletId}: {Amount}", id, model.Amount);
        return NoContent();
    }

    [HttpPost("{id:guid}/remove-funds")]
    public async Task<IActionResult> RemoveFunds(Guid id, [FromBody] RemoveFundsModel model)
    {
        await _mediator.Send(new RemoveFundsCommand(id, model.Amount, model.ProviderId, model.TransactionId));
        _logger.LogInformation("Funds removed from wallet {WalletId}: {Amount}", id, model.Amount);
        return NoContent();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetWalletResponseModel>> GetWallet(Guid id)
    {
        var wallet = await _mediator.Send(new GetWalletBalanceQuery(id));

        return Ok(new GetWalletResponseModel(wallet));
    }
}