
using LoyaltyCard.Domain.Entities;
using LoyaltyCard.Application.Interfaces;
using LoyaltyCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyCard.Infrastructure.Repositories;
public class LoyaltyCardRepository: ILoyaltyCardRepository
{
    private readonly LoyaltyCardDbContext _context;
    public LoyaltyCardRepository(LoyaltyCardDbContext context)
    {
        _context = context;        
    }
    public async Task<LoyaltyCardEntity?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken) 
    {
        return await _context.LoyaltyCards.AsNoTracking().FirstOrDefaultAsync(x=> x.CustomerId == customerId, cancellationToken);       
    }
    public async Task AddAsync(LoyaltyCardEntity loyaltyCard, CancellationToken cancellationToken) 
    {
        await _context.LoyaltyCards.AddAsync(loyaltyCard, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken); 
    }
    public async Task UpdateAsync(LoyaltyCardEntity loyaltyCard, CancellationToken cancellationToken) 
    {
        _context.LoyaltyCards.Update(loyaltyCard);
        await _context.SaveChangesAsync(cancellationToken);
    }         
}
