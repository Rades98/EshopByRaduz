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

app.MapGet("/create", async () =>
{
    var channel = GrpcChannel.ForAddress("http://localhost:5104");
    var client = new StockService.StockServiceClient(channel);

    var request = new ReserveStockRequest()
    {
        CheckoutReference = "BC995862-0013-4F42-91D1-93306BDCE959"
    };

    request.Items.Add(new StockItemWithQuantity() { Item = new() { Sku = "SKU-001", Variation = "Red-L" }, Quantity = 1 });
    request.Items.Add(new StockItemWithQuantity() { Item = new() { Sku = "SKU-002", Variation = "Blue-M" }, Quantity = 2 });

    var res = await client.ReserveStockAsync(request);

    return Results.Ok(res);
})
.WithName("create");

app.Run();

