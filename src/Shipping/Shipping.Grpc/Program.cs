using EshopByRaduz.ServiceDefaults;
using Microsoft.AspNetCore.Server.Kestrel.Core;

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

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();

await app.RunAsync();
