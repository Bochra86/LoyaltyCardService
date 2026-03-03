using LoyaltyCard.Application.Commands.UpdateLoyaltyCard;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LoyaltyCard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
      //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
      //services.AddMediatR(cfg =>cfg.RegisterServicesFromAssembly(typeof(UpdateLoyaltyCardHandler).Assembly));

        return services;
    }
}
