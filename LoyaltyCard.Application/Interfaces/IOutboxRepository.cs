using LoyaltyCard.Domain.Entities;

namespace LoyaltyCard.Application.Interfaces;

public interface IOutboxRepository
{
    Task<List<OutboxMessage>> GetUnprocessedAsync(int limit, CancellationToken token);
    Task AddAsync(OutboxMessage message, CancellationToken token);
    Task SaveChangesAsync(CancellationToken token);
}