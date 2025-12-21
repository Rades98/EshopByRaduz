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

    var request = new AssignToOrderRequest()
    {
        CheckoutReference = "BC995862-0013-4F42-91D1-93306BDCE959",
        OrderReference = "0CE104E0-3E7D-460C-85BB-4DCCCB7AA408"
    };

    var res = await client.AssignToOrderAsync(request);

    return Results.Ok(res);
})
.WithName("CreateOrder");

app.Run();
