using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pricing.Infrastructure.Common;
using Seeder;
using Seeder.Models;

namespace Pricing.Seed;

public static class SeedExtensions
{
    private static readonly string _basePath = Path.Combine(AppContext.BaseDirectory, "Seeds");

    public static async Task ApplySeed(this IServiceProvider serviceProvider, IHostEnvironment env)
    {
        var seedPath = Path.Combine(_basePath, "seed.yaml");

        if (!File.Exists(seedPath))
        {
            throw new FileNotFoundException($"YAML file not found: {seedPath}");
        }

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        using var loggerFactory = LoggerFactory.Create(builder => { });

        var options = new DbContextOptionsBuilder<PricingDbContext>()
            .UseSqlServer(
                configuration.GetConnectionString("PricingDatabase"),
                opts =>
                {
                    opts.CommandTimeout(3000);
                })
            .UseLoggerFactory(loggerFactory)
            .Options;

        using var db = new PricingDbContext(options);

        var seed = Seeder.SeedExtensions.GetDeserializer().Deserialize<SeedModel>(await File.ReadAllTextAsync(seedPath));

        foreach (var file in seed.Files.OrderBy(x => x.Order))
        {
            var sqlScript = await file.GenerateScript(_basePath, env);

            if (sqlScript is not null)
            {
                var x = await db.Database.ExecuteSqlRawAsync(sqlScript);
            }
        }
    }
}
