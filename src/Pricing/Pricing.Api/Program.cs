using EshopByRaduz.ServiceDefaults;
using Pricing.Api.Endpoints;
using Pricing.App.Common;
using Pricing.Infrastructure.Common;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.RegisterInfraStructure(builder.Configuration, builder.Environment);
builder.Services.RegisterApplicationLayer();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapUpsertPricingEndpoint();

app.MapOpenApi();
app.MapScalarApiReference();

await app.RunAsync();


