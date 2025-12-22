using Aspire.Hosting.Yarp.Transforms;
using EshopByRaduz.AppHost.Apps;

var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameterFromConfiguration("SqlPassword", "SqlPassword");

var sql = builder.AddSqlServer("sql", port: 10434)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPassword(password)
    .WithVolume("sql-data", "/var/opt/mssql");

var kafka = builder
        .AddKafka("kafka")
        .WithKafkaUI(kafkaUI => kafkaUI.WithHostPort(9100))
        .WithDataVolume("kafka-data", isReadOnly: false);

var catalog = builder.MapCatalog(kafka, sql);
var (stock, stockGrpc) = builder.MapStock(kafka, sql);
var pricingGrpc = builder.MapPricing(kafka, sql);
var (basket, basketGrpc) = builder.MapBasket(kafka, stockGrpc, pricingGrpc);
var payments = builder.MapPayments(kafka, sql);
var (shipping, shippinGrpc) = builder.MapShipping(kafka, sql);
var checkout = builder.MapCheckout(kafka, sql, stockGrpc, pricingGrpc, shippinGrpc);
var order = builder.MapOrder(kafka, sql);
var notifications = builder.MapNotifications(kafka);

builder.AddYarp("gateway")
    .WithReference(basket)
    .WithReference(catalog)
    .WithReference(checkout)
    .WithReference(order)
    .WithReference(shipping)
    .WithConfiguration(config =>
    {
        config.AddRoute("/basket/{**catch-all}", basket)
            .WithTransformPathRemovePrefix("/basket");

        config.AddRoute("/catalog/{**catch-all}", catalog)
            .WithTransformPathRemovePrefix("/catalog");

        config.AddRoute("/checkout/{**catch-all}", checkout)
            .WithTransformPathRemovePrefix("/checkout");

        config.AddRoute("/order/{**catch-all}", order)
            .WithTransformPathRemovePrefix("/order");

        config.AddRoute("/shipping/{**catch-all}", shipping)
            .WithTransformPathRemovePrefix("/shipping");
    });

builder.Build().Run();
