
namespace LoyaltyCard.Application.Dtos;

public class LoyaltyCardResponseDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public int Points { get; set; }
    public DateTime CreatedAt { get; set; }
}
