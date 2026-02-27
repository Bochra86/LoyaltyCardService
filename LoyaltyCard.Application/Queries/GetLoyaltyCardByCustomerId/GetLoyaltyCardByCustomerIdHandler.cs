using LoyaltyCard.Application.Interfaces;
using LoyaltyCard.Application.Dtos;

namespace LoyaltyCard.Application.Queries.GetLoyaltyCardByCustomerId;
public class GetLoyaltyCardByCustomerIdHandler
{
    private readonly ILoyaltyCardRepository _repository;
    private readonly ICacheService _cache;   
    public GetLoyaltyCardByCustomerIdHandler(ILoyaltyCardRepository repository, ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }
    public async Task<LoyaltyCardResponseDto> HandleAsync(GetLoyaltyCardByCustomerIdQuery query, CancellationToken token)
    {
        var key = $"loyaltycard:{query.CustomerId}";
        var cached = await _cache.GetAsync<LoyaltyCardResponseDto>(key);

        var loyaltyCard = await _repository.GetByCustomerIdAsync(query.CustomerId,token) ?? throw new KeyNotFoundException($"Loyalty card for customer {query.CustomerId} not found");

        var response = new LoyaltyCardResponseDto
        {
            Id = loyaltyCard.Id,
            CustomerId = loyaltyCard.CustomerId,
            Points = loyaltyCard.Points,
            CreatedAt = loyaltyCard.CreatedAt
        };

        await _cache.SetAsync(key, response,TimeSpan.FromMinutes(5));

        return response;
    }   
}