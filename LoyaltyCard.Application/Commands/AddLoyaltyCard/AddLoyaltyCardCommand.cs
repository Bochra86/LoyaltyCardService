using MediatR;

namespace LoyaltyCard.Application.Commands.AddLoyaltyCard;

public record AddLoyaltyCardCommand(Guid CustomerId) : IRequest<Guid>;