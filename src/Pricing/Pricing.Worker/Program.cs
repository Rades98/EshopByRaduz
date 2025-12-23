using EshopByRaduz.ServiceDefaults;
using InOutBox.Workers;
using Microsoft.EntityFrameworkCore;
using Pricing.App.Common;
using Pricing.Infrastructure.Common;
using Pricing.Seed;
using Pricing.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.RegisterInfraStructure(builder.Configuration, builder.Environment);
builder.Services.RegisterApplicationLayer(addInOutBoxOrchestrators: true);

builder.Services.AddHostedService<StockItemConsumer>();

builder.Services.AddInboxWorker();

builder.Services.AddOutboxWorker();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<PricingDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.ApplySeed();
}


await host.RunAsync();
