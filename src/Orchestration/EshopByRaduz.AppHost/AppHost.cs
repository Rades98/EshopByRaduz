using Aspire.Hosting.Yarp.Transforms;
using EshopByRaduz.AppHost.Domains;

var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameterFromConfiguration("SqlPassword", "SqlPassword");

var sql = builder.AddSqlServer("sql", port: 10434)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPassword(password)
    .WithVolume("sql-data", "/var/opt/mssql");

var kafka = builder
        .AddKafka("kafka")
        .WithKafkaUI(kafkaUI => kafkaUI.WithHostPort(9100))
        .WithDataVolume(isReadOnly: false);

var catalog = builder.MapCatalog(kafka, sql);
var (stock, stockGrpc) = builder.MapStock(kafka, sql);
var (basket, basketGrpc) = builder.MapBasket(kafka, stockGrpc);
var checkout = builder.MapCheckout(kafka, sql, stockGrpc);
var order = builder.MapOrder(kafka, sql);

builder.AddYarp("ingress")
    .WithReference(basket)
    .WithReference(catalog)
    .WithReference(checkout)
    .WithReference(order)
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
    });

builder.Build().Run();
