using EshopByRaduz.ServiceDefaults;
using Microsoft.EntityFrameworkCore;
using Regulatory.App.Common;
using Regulatory.Infrastructure.Common;
using Regulatory.Seed;
using Regulatory.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<CatalogGroupConsumer>();

builder.Services.RegisterInfraStructure(builder.Configuration, builder.Environment);
builder.Services.RegisterApplicationLayer(addInOutBoxOrchestrators: true);

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<RegulatoryDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.ApplySeed();
}

await host.RunAsync();
