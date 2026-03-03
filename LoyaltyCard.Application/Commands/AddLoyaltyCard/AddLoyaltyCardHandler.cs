using LoyaltyCard.Application.Dtos;
using LoyaltyCard.Application.IntegrationEvents;
using LoyaltyCard.Application.Interfaces;
using LoyaltyCard.Domain.Entities;
using MediatR;
using System.Text.Json;

namespace LoyaltyCard.Application.Commands.AddLoyaltyCard;

public class AddLoyaltyCardHandler : IRequestHandler<AddLoyaltyCardCommand, Guid>
{
    private readonly ILoyaltyCardRepository _repository;
    private readonly ICacheService _cache;
    private readonly IOutboxRepository _outbox;

    public AddLoyaltyCardHandler(ILoyaltyCardRepository repository, ICacheService cache, IOutboxRepository outbox)
    {
        _repository = repository;
        _cache = cache;
        _outbox = outbox;
    }
    public async Task<Guid> Handle(AddLoyaltyCardCommand command, CancellationToken token)
    {
        var loyaltyCard = new LoyaltyCardEntity(command.CustomerId);
        
        await _repository.AddAsync(loyaltyCard, token);

        var integrationEvent = new LoyaltyCardAddedIntegrationEvent(loyaltyCard.Id, loyaltyCard.CustomerId, loyaltyCard.Points);

        var message = OutboxMessage.Create(
            nameof(LoyaltyCardAddedIntegrationEvent),
            JsonSerializer.Serialize(integrationEvent)
        );
        await _outbox.AddAsync(message, token);
        await _outbox.SaveChangesAsync(token);


        await _cache.SetAsync($"loyaltycard:{loyaltyCard.CustomerId}",new LoyaltyCardResponseDto
            {
                Id = loyaltyCard.Id,
                CustomerId = loyaltyCard.CustomerId,
                Points = loyaltyCard.Points,
                CreatedAt = loyaltyCard.CreatedAt
            },
            TimeSpan.FromMinutes(10));

        return loyaltyCard.Id;
         
    }
}
