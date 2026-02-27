using LoyaltyCard.Application.Interfaces;
using LoyaltyCard.Domain.Entities;
using LoyaltyCard.Application.Dtos;

namespace LoyaltyCard.Application.Commands.AddLoyaltyCard;

public class AddLoyaltyCardHandler
{
    private readonly ILoyaltyCardRepository _repository;
    private readonly ICacheService _cache;

    public AddLoyaltyCardHandler(ILoyaltyCardRepository repository, ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }
    public async Task<Guid> HandleAsync(AddLoyaltyCardCommand command, CancellationToken token)
    {
        var loyaltyCard = new LoyaltyCardEntity(command.CustomerId);
        
        await _repository.AddAsync(loyaltyCard, token);

        var responseDto = new LoyaltyCardResponseDto
        {
            Id = loyaltyCard.Id,
            CustomerId = loyaltyCard.CustomerId,
            Points = loyaltyCard.Points,
            CreatedAt = loyaltyCard.CreatedAt
        };

        await _cache.SetAsync($"loyaltycard:{loyaltyCard.CustomerId}",  responseDto,TimeSpan.FromMinutes(10));

        return loyaltyCard.Id;
         
    }
}
