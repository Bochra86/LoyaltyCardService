namespace LoyaltyCard.Application.IntegrationEvents;

public record LoyaltyCardAddedIntegrationEvent(Guid LoyaltyCardId,Guid CustomerId,int Points);