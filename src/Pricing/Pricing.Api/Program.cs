using EshopByRaduz.ServiceDefaults;
using Microsoft.EntityFrameworkCore;
using Pricing.App.Common;
using Pricing.Infrastructure.Common;
using Pricing.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.RegisterInfraStructure(builder.Configuration, builder.Environment);
builder.Services.RegisterApplicationLayer(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

if (args.Contains("--seed"))
{
    var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<PricingDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        await db.Database.MigrateAsync();
        logger.LogInformation("SQL migrations applied succesfully");
    }
    catch (Exception e)
    {
        logger.LogError(e, "SQL migrations failed");
        scope.Dispose();
        Environment.Exit(1);
        return;
    }

    try
    {
        await scope.ServiceProvider.ApplySeed(app.Environment);
        logger.LogInformation("SQL seed applied succesfully");
    }
    catch (Exception e)
    {
        logger.LogError(e, "SQL seed failed");
        scope.Dispose();
        Environment.Exit(1);
        return;
    }

    scope.Dispose();

    Environment.Exit(0);
    return;
}

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



await app.RunAsync();

