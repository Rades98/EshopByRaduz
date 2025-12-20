using EshopByRaduz.ServiceDefaults;
using Grpc.Net.Client;
using Stock.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/aval", async () =>
{
    var channel = GrpcChannel.ForAddress("http://localhost:5104");
    var client = new StockCount.StockCountClient(channel);

    var res = await client.GetStockCountAsync(new StockCountRequest { SKU = "SKU-001", VariationId = "Red-L" });

    return Results.Ok(res);
})
.WithName("aval");


app.MapGet("/x", () =>
{
    return Results.Ok("Cuuuus");
})
.WithName("Greetings2");

app.Run();

