using System.Text.Json;
using System.Text.Json.Serialization;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PosOptimizer.Application.Extensions;
using PosOptimizer.Infrastructure.DatabaseContext;
using PosOptimizer.Infrastructure.Extensions;
using PosOptimizer.Job;
using PosOptimizer.Job.Extensions;
using PosOptimizer.MockApiClient.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services
    .AddServices()
    .AddDatabase(builder.Configuration)
    .AddRedis(builder.Configuration)
    .AddMockApiClient(builder.Configuration)
    .AddHangfireService(builder.Configuration);

builder.Services.AddTransient<PosRatioFetchJob>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    db.Database.Migrate();

    var recurring = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    
    recurring.AddOrUpdate<PosRatioFetchJob>(
        "ratio-sync-job",
        x => x.Execute(),
        "0 0 * * *"
    );
}

app.Run();