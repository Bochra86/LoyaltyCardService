using LoyaltyCard.Application;
using LoyaltyCard.Infrastructure;
using LoyaltyCard.Worker.Background;    


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<OutboxProcessor>();

var host = builder.Build();
await host.RunAsync();