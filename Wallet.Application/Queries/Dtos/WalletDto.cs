namespace Wallet.Application.Queries.Dtos;

public class WalletDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
    public IReadOnlyCollection<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
}