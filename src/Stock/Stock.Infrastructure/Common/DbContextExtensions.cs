using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Stock.Infrastructure.Common;

/// <summary>
/// "UoW" - encapsulates db requests with transaction
/// </summary>
internal static class DbContextExtensions
{
    internal static async Task<T?> RunWithinTransaction<T>(
        this StockDbContext dbContext,
        Func<StockDbContext, Task<T?>> request,
        CancellationToken cancellationToken)
    {
        if (dbContext.Database.CurrentTransaction != null)
        {
            var savepointName = GetSavepointName();
            var currentTransaction = dbContext.Database.CurrentTransaction;

            try
            {
                await currentTransaction.CreateSavepointAsync(savepointName, cancellationToken);
                var result = await request(dbContext);
                await dbContext.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (Exception)
            {
                await currentTransaction.RollbackToSavepointAsync(savepointName, cancellationToken);
                throw;
            }
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await request(dbContext);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return default;
        }
    }

    internal static async Task<bool> AddWithinTransaction<T>(
        this StockDbContext dbContext,
        Func<StockDbContext, ValueTask<EntityEntry<T>>> request,
        CancellationToken cancellationToken)
        where T : class
    {
        if (dbContext.Database.CurrentTransaction != null)
        {
            var savepointName = GetSavepointName();
            var currentTransaction = dbContext.Database.CurrentTransaction;

            try
            {
                await currentTransaction.CreateSavepointAsync(savepointName, cancellationToken);
                var result = await request(dbContext);
                await dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception)
            {
                await currentTransaction.RollbackToSavepointAsync(savepointName, cancellationToken);
                throw;
            }
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await request(dbContext);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    internal static async Task<bool> UpdateWithinTransaction<T>(
        this StockDbContext dbContext,
        Func<StockDbContext, Task> request,
        CancellationToken cancellationToken)
        where T : class
    {
        if (dbContext.Database.CurrentTransaction != null)
        {
            var savepointName = GetSavepointName();
            var currentTransaction = dbContext.Database.CurrentTransaction;

            try
            {
                await currentTransaction.CreateSavepointAsync(savepointName, cancellationToken);
                await request(dbContext);
                await dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception)
            {
                await currentTransaction.RollbackToSavepointAsync(savepointName, cancellationToken);
                throw;
            }
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await request(dbContext);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    private static string GetSavepointName()
        => $"SP_{Guid.NewGuid():N}"[..30];
}
