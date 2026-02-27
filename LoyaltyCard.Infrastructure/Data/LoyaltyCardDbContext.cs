using LoyaltyCard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace LoyaltyCard.Infrastructure.Data;

public class LoyaltyCardDbContext: DbContext
{
    public LoyaltyCardDbContext(DbContextOptions<LoyaltyCardDbContext> options) : base(options)
    {
    }
    public DbSet<LoyaltyCardEntity> LoyaltyCards => Set<LoyaltyCardEntity>();

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
    }
}
