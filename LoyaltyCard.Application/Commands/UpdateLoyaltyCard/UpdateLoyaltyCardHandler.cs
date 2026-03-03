using LoyaltyCard.Application.Dtos;
using LoyaltyCard.Application.IntegrationEvents;
using LoyaltyCard.Application.Interfaces;  
using LoyaltyCard.Domain.Entities;
using MediatR;

using System.Text.Json; 

namespace LoyaltyCard.Application.Commands.UpdateLoyaltyCard;

public class UpdateLoyaltyCardHandler : IRequestHandler<UpdateLoyaltyCardCommand, Guid>
{
    private readonly ILoyaltyCardRepository _repository;
    private readonly ICacheService _cache;
    private readonly IOutboxRepository _outbox;

    public UpdateLoyaltyCardHandler(ILoyaltyCardRepository repository, ICacheService cache, IOutboxRepository outbox)
    {
        _repository = repository;
        _cache = cache;
        _outbox = outbox;
    }
    public async Task<Guid> Handle(UpdateLoyaltyCardCommand command, CancellationToken cancellationToken)
    {
        var loyaltyCard = await _repository.GetByCustomerIdAsync(command.CustomerId, cancellationToken)?? throw new KeyNotFoundException("Loyalty card not found.");
        
        loyaltyCard.AddPoints(command.PointsToAdd);   
        
        await _repository.UpdateAsync(loyaltyCard, cancellationToken);

        
        var integrationEvent = new LoyaltyCardUpdatedIntegrationEvent(loyaltyCard.Id, loyaltyCard.CustomerId, loyaltyCard.Points);

        var message = OutboxMessage.Create(
            nameof(LoyaltyCardUpdatedIntegrationEvent),
            JsonSerializer.Serialize(integrationEvent)
        );

        await _outbox.AddAsync(message, cancellationToken);
        await _outbox.SaveChangesAsync(cancellationToken);

        await _cache.SetAsync($"loyaltycard:{command.CustomerId}",new UpdateLoyaltyCardPointsDto
            {
                Points = loyaltyCard.Points
            },
            TimeSpan.FromMinutes(10));

        return loyaltyCard.Id;
    }   

}
