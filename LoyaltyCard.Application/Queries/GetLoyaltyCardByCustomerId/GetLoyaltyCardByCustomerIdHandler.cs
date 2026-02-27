using LoyaltyCard.Application.Interfaces;
using LoyaltyCard.Application.Dtos;

namespace LoyaltyCard.Application.Queries.GetLoyaltyCardByCustomerId;

public class GetLoyaltyCardByCustomerIdHandler
{
    private readonly ILoyaltyCardRepository _repository;
    public GetLoyaltyCardByCustomerIdHandler(ILoyaltyCardRepository repository)
    {
        _repository = repository;
    }
    public async Task<LoyaltyCardResponseDto> HandleAsync(GetLoyaltyCardByCustomerIdQuery query, CancellationToken token)
    {
        var loyaltyCard = await _repository.GetByCustomerIdAsync(query.CustomerId,token);
        if (loyaltyCard == null)
            throw new KeyNotFoundException($"Loyalty card for customer {query.CustomerId} not found");
        return new LoyaltyCardResponseDto
        {
            Id = loyaltyCard.Id,
            CustomerId = loyaltyCard.CustomerId,
            Points = loyaltyCard.Points,
            CreatedAt = loyaltyCard.CreatedAt
        };
    }   
}
