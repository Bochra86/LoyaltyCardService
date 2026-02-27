using LoyaltyCard.Application.Dtos;
using LoyaltyCard.Application.Interfaces;  

namespace LoyaltyCard.Application.Commands.UpdateLoyaltyCard;

public class UpdateLoyaltyCardHandler
{
    private readonly ILoyaltyCardRepository _repository;
    private readonly ICacheService _cache;  
    public UpdateLoyaltyCardHandler(ILoyaltyCardRepository repository, ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }
    public async Task<Guid> HandleAsync(UpdateLoyaltyCardCommand command, CancellationToken cancellationToken)
    {
        var loyaltyCard = await _repository.GetByCustomerIdAsync(command.CustomerId, cancellationToken)?? throw new KeyNotFoundException("Loyalty card not found.");
        
        loyaltyCard.AddPoints(command.PointsToAdd);

        var updatedDto = new UpdateLoyaltyCardPointsDto
        {
            Points = loyaltyCard.Points,
        };      

        await _repository.UpdateAsync(loyaltyCard, cancellationToken);

        await _cache.SetAsync( $"loyaltycard:{command.CustomerId}",updatedDto,TimeSpan.FromMinutes(10));

        return loyaltyCard.Id;
    }   

}
