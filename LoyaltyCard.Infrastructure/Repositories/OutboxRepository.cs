using LoyaltyCard.Application.Interfaces;
using LoyaltyCard.Domain.Entities;
using LoyaltyCard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class OutboxRepository : IOutboxRepository
{
    private readonly LoyaltyCardDbContext _context;

    public OutboxRepository(LoyaltyCardDbContext context)
    {
        _context = context;
    }
    public async Task<List<OutboxMessage>> GetUnprocessedAsync(int limit,CancellationToken cancellationToken)
    {
        return await _context.OutboxMessages
            .Where(x => !x.Processed)
            .OrderBy(x => x.OccurredOn)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
    public async Task AddAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        await _context.OutboxMessages.AddAsync(message, cancellationToken);
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}