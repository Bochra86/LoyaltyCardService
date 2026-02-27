using LoyaltyCard.Application.Interfaces;  

namespace LoyaltyCard.Application.Commands.UpdateLoyaltyCard;

public class UpdateLoyaltyCardHandler
{
    private readonly ILoyaltyCardRepository _repository;
    public UpdateLoyaltyCardHandler(ILoyaltyCardRepository repository)
    {
        _repository = repository;
    }
    public async Task<Guid> HandleAsync(UpdateLoyaltyCardCommand command, CancellationToken cancellationToken)
    {
        var loyaltyCard = await _repository.GetByCustomerIdAsync(command.CustomerId, cancellationToken);
        if (loyaltyCard == null)
        {
            throw new KeyNotFoundException("Loyalty card not found.");
        }
        loyaltyCard.AddPoints(command.PointsToAdd);
        await _repository.UpdateAsync(loyaltyCard, cancellationToken);
        return loyaltyCard.Id;
    }   

}
