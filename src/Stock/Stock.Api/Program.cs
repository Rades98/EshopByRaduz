using EshopByRaduz.ServiceDefaults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.App.Common;
using Stock.Infrastructure.Common;
using Stock.Seed;

var builder = WebApplication.CreateBuilder(args);


builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.RegisterInfraStructure(builder.Configuration, builder.Environment);

var app = builder.Build();

if (args.Contains("--seed"))
{
    var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<StockDbContext>();
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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/send", async ([FromServices] IEventPublisher ev) =>
{
    await ev.PublishAsync(new Shit("dlouhan"), default);

    return Results.Ok("Cuuuus");
})
.WithName("Greetings");


app.Run();

record Shit(string Name);
