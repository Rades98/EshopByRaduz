using Aspire.Hosting.Yarp.Transforms;

var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameterFromConfiguration("SqlPassword", "SqlPassword");

var sql = builder.AddSqlServer("sql", port: 10434)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPassword(password)
    .WithVolume("sql-data", "/var/opt/mssql");

var redisCahe = builder.AddRedis("RedisCache")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithRedisInsight();

var redisDb = builder.AddRedis("RedisDb")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithRedisInsight();

var kafka = builder
        .AddKafka("kafka")
        .WithKafkaUI(kafkaUI => kafkaUI.WithHostPort(9100))
        .WithDataVolume(isReadOnly: false);

var stockDatabase = sql.AddDatabase("StockDatabase");
var catalogDatabase = sql.AddDatabase("CatalogDatabase");
var orderDatabase = sql.AddDatabase("OrderDatabase");

var stockSeed = builder.AddProject<Projects.Stock_Api>("stockapiseed")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
    .WithArgs("--seed")
    .WithHttpEndpoint(port: 5555, name: "stockSeed")
    .WithReference(stockDatabase)
        .WaitFor(stockDatabase)
    .WithEnvironment("ConnectionStrings__Sql", stockDatabase);

var stock = builder.AddProject<Projects.Stock_Api>("stockapi")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
    .WaitForCompletion(stockSeed)
    .WithReference(stockDatabase)
        .WaitFor(stockDatabase)
    .WithReference(kafka)
        .WaitFor(kafka)
    .WithEnvironment("ConnectionStrings__Sql", stockDatabase);

var stockGrpc = builder.AddProject<Projects.Stock_Grpc>("stock-grpc")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
    .WaitForCompletion(stockSeed)
    .WithReference(stockDatabase)
        .WaitFor(stockDatabase)
    .WithEnvironment("ConnectionStrings__Sql", stockDatabase);

var basket = builder.AddProject<Projects.Basket_Api>("basketapi")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
    .WithReference(redisDb)
        .WaitFor(redisDb)
    .WithEnvironment("ConnectionStrings__Redis", redisDb)
    .WithReference(stockGrpc)
        .WaitFor(stockGrpc);

var catalog = builder.AddProject<Projects.Catalog_Api>("catalogapi")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
    .WithReference(catalogDatabase)
        .WaitFor(catalogDatabase)
    .WithEnvironment("ConnectionStrings__Sql", catalogDatabase)
    .WithReference(redisCahe)
        .WaitFor(redisCahe)
    .WithEnvironment("ConnectionStrings__Redis", redisCahe);

var checkout = builder.AddProject<Projects.Checkout_Api>("checkoutapi")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName);

var order = builder.AddProject<Projects.Order_Api>("orderapi")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
    .WithReference(orderDatabase)
        .WaitFor(orderDatabase)
    .WithEnvironment("ConnectionStrings__Sql", orderDatabase);

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
