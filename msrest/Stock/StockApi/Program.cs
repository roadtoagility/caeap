using Stock.Business;
using Stock.Messaging.Kafka;
using Stock.Persistence;
using Stock.Supporting;
using Microsoft.AspNetCore.Http.Json;
using Stock.RestAPI.ApiEndpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables("ECOMMERCE_");

builder.Services.AddSupporting();
builder.Services.AddProducers();
builder.Services.AddRepositories();
builder.Services.AddHandlers();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddHealthChecks();

builder.Services.Configure<JsonOptions>(o =>
{
    o.SerializerOptions.IgnoreReadOnlyProperties = true;
});


var app = builder.Build();

// reportando problemas de forma padronizada
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/handle-errrors?view=aspnetcore-8.0#problem-details
app.UseExceptionHandler(exceptionHandlerApp 
    => exceptionHandlerApp.Run(async context 
        => await Results.Problem()
            .ExecuteAsync(context)));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}else
{
    app.UseHsts();
}

EndpointRoutes.StateChangeApis(app);

app.Run();

//It's for integration testing propose :(
// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
public partial class Program { }