namespace LoyaltyCard.Application.Commands.UpdateLoyaltyCard;

public record UpdateLoyaltyCardCommand(Guid CustomerId, int PointsToAdd);    
