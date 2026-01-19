using Hangfire;
using Infrastructure.Logging;
using Infrastructure.MediatR;
using NotificationService.Application;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.AddInfrastructureLogging();

// ------ Configure services ------ //
builder.Services
    .AddApplicationServices(configuration)
    .AddPipelineBehaviours()
    .AddKafkaConsumers(configuration)
    .AddHangfireWithRedis(configuration)
    ;

// ------ Configure WebApplication ------ //
var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseHangfireDashboard();

app.Run();
