using FluentValidation;
using MediatR;
using PosOptimizer.Api.Middlewares;
using PosOptimizer.Application;
using PosOptimizer.Application.Extensions;
using PosOptimizer.Application.Models.Queries;
using PosOptimizer.Infrastructure.Extensions;
using PosOptimizer.MockApiClient.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services
    .AddServices()
    .AddDatabase(builder.Configuration)
    .AddRedis(builder.Configuration)
    .AddMockApiClient(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyReference).Assembly);
});

builder.Services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyReference).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.MapPost("/calculate-pos", async (CalculatePosQuery query, IMediator mediator) =>
{
    var result = await mediator.Send(query);
    return Results.Ok(result);
});

app.Run();