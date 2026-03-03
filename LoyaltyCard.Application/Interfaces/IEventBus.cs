namespace LoyaltyCard.Application.Interfaces;

public interface IEventBus
{
    Task ProduceAsync<TEvent>(TEvent @event) where TEvent : class;
}
