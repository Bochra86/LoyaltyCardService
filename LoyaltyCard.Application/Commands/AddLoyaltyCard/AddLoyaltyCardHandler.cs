using LoyaltyCard.Application.Interfaces;
using LoyaltyCard.Domain.Entities;

namespace LoyaltyCard.Application.Commands.AddLoyaltyCard;

public class AddLoyaltyCardHandler
{
        private readonly ILoyaltyCardRepository _repository;
        public AddLoyaltyCardHandler(ILoyaltyCardRepository repository)
        {
            _repository = repository;
        }
        public async Task<Guid> HandleAsync(AddLoyaltyCardCommand command, CancellationToken token)
        { 
            var loyaltyCard = new LoyaltyCardEntity(command.CustomerId);
            await _repository.AddAsync(loyaltyCard, token);
            return loyaltyCard.Id;
         
        }
}
