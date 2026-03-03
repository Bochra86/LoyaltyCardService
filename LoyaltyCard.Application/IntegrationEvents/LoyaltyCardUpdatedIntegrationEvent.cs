namespace LoyaltyCard.Application.IntegrationEvents;

public record LoyaltyCardUpdatedIntegrationEvent(Guid LoyaltyCardId, Guid CustomerId, int Points);