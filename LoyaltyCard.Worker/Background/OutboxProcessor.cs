using Confluent.Kafka;
using LoyaltyCard.Application.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LoyaltyCard.Worker.Background;

public class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<OutboxProcessor> _logger;

    private const string Topic = "loyaltycard-events";
    private const int BatchSize = 50;
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(5);

    public OutboxProcessor(
        IServiceScopeFactory scopeFactory,
        IProducer<string, string> producer,
        ILogger<OutboxProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _producer = producer;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Processor started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var repository = scope.ServiceProvider
                    .GetRequiredService<IOutboxRepository>();

                var messages = await repository
                    .GetUnprocessedAsync(limit: BatchSize, stoppingToken);

                if (!messages.Any())
                {
                    await Task.Delay(Delay, stoppingToken);
                    continue;
                }

                foreach (var message in messages)
                {
                    try
                    {
                        var result = await _producer.ProduceAsync(
                            Topic,
                            new Message<string, string>
                            {
                                Key = message.Id.ToString(),
                                Value = message.Payload
                            },
                            stoppingToken);

                        _logger.LogInformation(
                            "Outbox message {MessageId} published to {Topic} at offset {Offset}",
                            message.Id,
                            Topic,
                            result.Offset);

                        message.MarkAsProcessed();
                    }
                    catch (ProduceException<string, string> ex)
                    {
                        _logger.LogError(
                            ex,
                            "Kafka publish failed for message {MessageId}",
                            message.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Unexpected error processing message {MessageId}",
                            message.Id);
                    }
                }

                await repository.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Critical error in OutboxProcessor loop.");
            }

            await Task.Delay(Delay, stoppingToken);
        }

        _logger.LogInformation("Outbox Processor stopped.");
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Flushing Kafka producer...");
        _producer.Flush(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}