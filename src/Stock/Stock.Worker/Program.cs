using EshopByRaduz.ServiceDefaults;
using InOutBox.Workers;
using Stock.App.Common;
using Stock.Infrastructure.Common;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.RegisterInfraStructure(builder.Configuration, builder.Environment);
builder.Services.RegisterApplicationLayer(addInOutBoxOrchestrators: true);

builder.Services.AddOutboxWorker();

var host = builder.Build();
host.Run();
