using Basket.Api;
using Basket.Api.Endpoints;
using EshopByRaduz.ServiceDefaults;
using Scalar.AspNetCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisDb")!));

builder.Services.AddSingleton<KafkaEventPublisher>();
builder.Services.AddSingleton<StockGrpcService>();

var app = builder.Build();

app.MapDefaultEndpoints();

app
    .MapCreateBasketEndpoint()
    .MapUpdateBasketEndpoint()
    .MapGetBasketEndpoint();

app.MapOpenApi();
app.MapScalarApiReference();

app.Run();

