using EshopByRaduz.ServiceDefaults;
using Pricing.App.Common;
using Pricing.Consumer;
using Pricing.Infrastructure.Common;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.RegisterInfraStructure(builder.Configuration, builder.Environment);
builder.Services.RegisterApplicationLayer(builder.Configuration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

await host.RunAsync();
