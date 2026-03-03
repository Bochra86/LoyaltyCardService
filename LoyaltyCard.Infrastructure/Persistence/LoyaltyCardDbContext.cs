using LoyaltyCard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace LoyaltyCard.Infrastructure.Persistence;

public class LoyaltyCardDbContext: DbContext
{
    public LoyaltyCardDbContext(DbContextOptions<LoyaltyCardDbContext> options) : base(options)
    {
    }
    public DbSet<LoyaltyCardEntity> LoyaltyCards => Set<LoyaltyCardEntity>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LoyaltyCardEntity>(entity =>
        {
            entity.ToTable("loyalty_cards"); 
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CustomerId);

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Points).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<OutboxMessage>(x =>
        {
            x.HasKey(o => o.Id);
            x.Property(o => o.Type).IsRequired();
            x.Property(o => o.Payload).IsRequired();
        });
    }
}
