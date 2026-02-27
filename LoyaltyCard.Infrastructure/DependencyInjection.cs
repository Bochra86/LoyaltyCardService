using LoyaltyCard.Application.Interfaces;
using LoyaltyCard.Infrastructure.Repositories;
using LoyaltyCard.Infrastructure.Persistence;
using LoyaltyCard.Infrastructure.Redis;  

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;


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
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(config.GetConnectionString("Redis") ?? throw new InvalidOperationException("Redis Connection String must be different than null")));    
        services.AddScoped<ICacheService, RedisCache>();

        return services;
    }

}
