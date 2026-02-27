namespace LoyaltyCard.Domain.Entities;

public class LoyaltyCardEntity
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public int Points { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private LoyaltyCardEntity() { }
    public LoyaltyCardEntity(Guid customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        Points = 0;
        CreatedAt = DateTime.UtcNow;
    }
    // Business Rules inside the Entity:
    public void AddPoints(int points)
    {
        if (points <= 0)
            throw new ArgumentException("Points must be greater than zero");
        Points += points;

    }
}