namespace Wallet.Application.Queries.Dtos;

public class TransactionDto
{
    public Guid Id { get;  set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public DateTime CreatedAt { get; set; }
}