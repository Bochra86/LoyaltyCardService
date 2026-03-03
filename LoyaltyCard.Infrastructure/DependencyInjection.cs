using LoyaltyCard.Application.Interfaces;
using LoyaltyCard.Infrastructure.Persistence;
using LoyaltyCard.Infrastructure.Repositories;
using LoyaltyCard.Infrastructure.Redis;  
using LoyaltyCard.Infrastructure.kafka;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Confluent.Kafka;


namespace LoyaltyCard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        //1.DB 
        services.AddDbContext<LoyaltyCardDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("PostgreSQL")));
        services.AddScoped<ILoyaltyCardRepository, LoyaltyCardRepository>();

        //2.Redis
        services.AddSingleton<IConnectionMultiplexer>(sp =>ConnectionMultiplexer.Connect(config.GetConnectionString("Redis") ?? throw new InvalidOperationException("Redis Connection String must be different than null")));    
        services.AddScoped<ICacheService, RedisCache>();

        //3.Kafka
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = config["Kafka:BootstrapServers"]
        };

        services.AddSingleton<IProducer<Null, string>>(new ProducerBuilder<Null, string>(producerConfig).Build());

        services.AddSingleton<IEventBus, KafkaProducer>();

        services.AddScoped<IOutboxRepository, OutboxRepository>();
        

        return services;
    }

}
