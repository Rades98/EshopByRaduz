using EshopByRaduz.ServiceDefaults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.Api.Common.Outbox;
using Stock.App.Common;
using Stock.App.StockItems.StockUnits.AddStockUnit;
using Stock.Infrastructure.Common;
using Stock.Seed;

var builder = WebApplication.CreateBuilder(args);


builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.RegisterInfraStructure(builder.Configuration, builder.Environment);
builder.Services.RegisterApplicationLayer(builder.Configuration);

builder.Services.AddHostedService<OutboxWorker>();

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

app.MapGet("/addNew", async ([FromServices] IMediator mediator) =>
{
    var res = await mediator.Send<AddStockUnitCommandResult>(new AddStockUnitCommand("SN-001-07", "SKU-001", "Red-L", Guid.Parse("47053D9C-5756-4630-AF23-142DAAD8844C")));

    return Results.Ok(res);
})
.WithName("Greetings");


await app.RunAsync();

record Shit(string Name);
