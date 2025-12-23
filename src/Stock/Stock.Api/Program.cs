using EshopByRaduz.ServiceDefaults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Stock.App.Common;
using Stock.App.StockItems.StockUnits.AddStockUnit;
using Stock.Infrastructure.Common;
using Stock.Seed;

var builder = WebApplication.CreateBuilder(args);


builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.RegisterInfraStructure(builder.Configuration, builder.Environment);
builder.Services.RegisterApplicationLayer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<StockDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.ApplySeed();
}

app.MapDefaultEndpoints();

app.MapGet("/addNew", async ([FromServices] IMediator mediator) =>
{
    var res = await mediator.Send<AddStockUnitCommandResult>(new AddStockUnitCommand("SN-001-07", "SKU-001", "Red-L", Guid.Parse("47053D9C-5756-4630-AF23-142DAAD8844C")));

    return Results.Ok(res);
})
.WithOpenApi()
.WithName("Greetings");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

await app.RunAsync();

record Shit(string Name);
