
using LoyaltyCard.Domain.Entities;
namespace LoyaltyCard.Application.Interfaces;

public interface ILoyaltyCardRepository
{
    Task<LoyaltyCardEntity?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);
    Task AddAsync(LoyaltyCardEntity loyaltyCard, CancellationToken cancellationToken);
    Task UpdateAsync(LoyaltyCardEntity loyaltyCard, CancellationToken cancellationToken);
}
