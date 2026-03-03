using LoyaltyCard.Application.Interfaces;
using Confluent.Kafka;
using System.Text.Json;

namespace LoyaltyCard.Infrastructure.kafka;
public class KafkaProducer : IEventBus
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer(IProducer<Null, string> producer)
    {
        _producer = producer;
    }

    public async Task ProduceAsync<TEvent>(TEvent @event) where TEvent : class
    {
        var topic = typeof(TEvent).Name.ToLower();

        var message = new Message<Null, string>
        {
            Value = JsonSerializer.Serialize(@event)
        };

        await _producer.ProduceAsync(topic, message);
    }
}