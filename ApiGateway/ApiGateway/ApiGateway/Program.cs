using ApiGateway.Extensions;
using Auth.Authentication;
using Infrastructure.HealthCheck;
using Infrastructure.Logging;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.AddInfrastructureLogging();

configuration.AddGatewayConfiguration(builder.Environment.EnvironmentName);
configuration.ValidateGatewayConfiguration();

builder
    .Services
    .AddCorsPolicy(configuration)
    .AddApiGateway(configuration)
    .AddRateLimit(configuration)
    .AddAuth(configuration)
    .AddInfrastructureHealthChecks(_ => { })
    ;

builder.Services.AddRateLimiter();

var app = builder.Build();

app.UseRateLimiter();
app.UseCorsPolicy();
app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();

app.Run();
