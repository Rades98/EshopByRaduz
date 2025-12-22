using Basket.Api.Endpoints;
using Basket.Api.Services;
using EshopByRaduz.ServiceDefaults;
using Kafka;
using Scalar.AspNetCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisDb")!));

builder.Services.AddKafkaPublisher();
builder.Services.AddSingleton<StockGrpcService>();
builder.Services.AddSingleton<PricingGrpcService>();

var app = builder.Build();

app.MapDefaultEndpoints();

app
    .MapUpdateBasketEndpoint()
    .MapGetBasketEndpoint();

app.MapOpenApi();
app.MapScalarApiReference();

await app.RunAsync();

