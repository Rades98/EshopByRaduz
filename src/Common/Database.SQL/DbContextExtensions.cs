using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Database.SQL;

/// <summary>
/// "UoW" - encapsulates db requests with transaction
/// </summary>
public static class DbContextExtensions
{
    public static async Task<T?> RunWithinTransaction<T, TContext>(
        this TContext dbContext,
        Func<TContext, Task<T?>> request,
        CancellationToken cancellationToken)
        where TContext : DbContext
    {
        _ = dbContext is null ? throw new ArgumentNullException(nameof(dbContext)) : "";
        _ = request is null ? throw new ArgumentNullException(nameof(request)) : "";

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

    public static async Task<bool> AddWithinTransaction<T, TContext>(
        this TContext dbContext,
        Func<TContext, ValueTask<EntityEntry<T>>> request,
        CancellationToken cancellationToken)
        where T : class
        where TContext : DbContext
    {
        _ = dbContext is null ? throw new ArgumentNullException(nameof(dbContext)) : "";
        _ = request is null ? throw new ArgumentNullException(nameof(request)) : "";

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

    public static async Task<bool> AddManyWithinTransaction<T, TContext>(
        this TContext dbContext,
        Func<TContext, ValueTask<IEnumerable<EntityEntry<T>>>> request,
        CancellationToken cancellationToken)
        where T : class
        where TContext : DbContext
    {
        _ = dbContext is null ? throw new ArgumentNullException(nameof(dbContext)) : "";
        _ = request is null ? throw new ArgumentNullException(nameof(request)) : "";

        if (dbContext.Database.CurrentTransaction != null)
        {
            var savePointName = GetSavepointName();
            var currentTransaction = dbContext.Database.CurrentTransaction;

            try
            {
                await currentTransaction.CreateSavepointAsync(savePointName, cancellationToken);
                var result = await request(dbContext);
                await dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception)
            {
                await currentTransaction.RollbackToSavepointAsync(savePointName, cancellationToken);
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

    public static async Task<bool> UpdateWithinTransaction<T, TContext>(
        this TContext dbContext,
        Func<TContext, Task> request,
        CancellationToken cancellationToken)
        where T : class
        where TContext : DbContext
    {
        _ = dbContext is null ? throw new ArgumentNullException(nameof(dbContext)) : "";
        _ = request is null ? throw new ArgumentNullException(nameof(request)) : "";

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
