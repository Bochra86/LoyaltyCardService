using LoyaltyCard.Application.Interfaces;
using LoyaltyCard.Infrastructure.Repositories;
using LoyaltyCard.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace LoyaltyCard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        //1.DB 
        services.AddDbContext<LoyaltyCardDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("PostgreSQL")));
        services.AddScoped<ILoyaltyCardRepository, LoyaltyCardRepository>();


        return services;
    }

}
