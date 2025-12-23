using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Seeder;
using Seeder.Models;
using Stock.Infrastructure.Common;

namespace Stock.Seed;

public static class SeedExtensions
{
    private static readonly string _basePath = Path.Combine(AppContext.BaseDirectory, "Seeds");

    public static async Task ApplySeed(this IServiceProvider serviceProvider)
    {
        var seedPath = Path.Combine(_basePath, "seed.yaml");

        if (!File.Exists(seedPath))
        {
            throw new FileNotFoundException($"YAML file not found: {seedPath}");
        }

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        using var loggerFactory = LoggerFactory.Create(builder => { });

        var options = new DbContextOptionsBuilder<StockDbContext>()
            .UseSqlServer(
                configuration.GetConnectionString("StockDatabase"),
                opts =>
                {
                    opts.CommandTimeout(3000);
                })
            .UseLoggerFactory(loggerFactory)
            .Options;

        using var db = new StockDbContext(options);

        var seed = Seeder.SeedExtensions.Deserializer.Deserialize<SeedModel>(await File.ReadAllTextAsync(seedPath));

        foreach (var file in seed.Files.OrderBy(x => x.Order))
        {
            var sqlScript = await file.GenerateScript(_basePath);

            if (sqlScript is not null)
            {
                var x = await db.Database.ExecuteSqlRawAsync(sqlScript);
            }
        }
    }
}
