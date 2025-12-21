using EshopByRaduz.ServiceDefaults;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Stock.App.Common;
using Stock.Grpc.Services;
using Stock.Infrastructure.Common;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    // There is no https here since i thing that it is not matter of app,
    // but gRPC needs h2c, so we set http2 here to fulfill handshake needs
    options.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.AddServiceDefaults();

builder.Services.AddGrpc();

builder.Services.RegisterInfraStructure(builder.Configuration, builder.Environment);
builder.Services.RegisterApplicationLayer(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGrpcService<StockService>();

app.MapGet("/", () => "This is gRPC Server :) ");

app.Run();
