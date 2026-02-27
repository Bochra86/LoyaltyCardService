using LoyaltyCard.Infrastructure;

//1. Create a WebApplication builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();


//2. Build the WebApplication
var app = builder.Build();


app.UseHttpsRedirection();

app.MapControllers();

app.Run();

