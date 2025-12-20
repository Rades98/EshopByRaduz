using Microsoft.EntityFrameworkCore;
using Stock.App.StockItems;
using Stock.Domain.StockItems;
using Stock.Domain.StockItems.StockUnits;
using Stock.Infrastructure.Common;
using Stock.Infrastructure.StockItems.StockUnits;

namespace Stock.Infrastructure.StockItems
{
    internal class StockItemRepo(StockDbContext context) : IStockItemRepo
    {
        public Task AddAsync(StockItemAggregate aggregate, CancellationToken cancellationToken) => throw new NotImplementedException();

        public Task<StockItemAggregate?> GetAsync(Guid stockItemId, CancellationToken cancellationToken)
            => context.StockItems
                .AsNoTracking()
                .Where(x => x.Id == stockItemId)
                .Take(1)
                .Select(res =>
                    StockItemAggregate.Rehydrate(
                        res.Id,
                        res.Units.Select(x =>
                            StockUnitModel.Rehydrate(
                                x.Id,
                                x.WarehouseId,
                                SerialNumberValueObject.Create(x.SerialNumber).Value!,
                                x.IsLocked,
                                x.LockedUntil))))
                .SingleAsync(cancellationToken)!;

        public async Task SaveAsync(StockItemAggregate aggregate, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(aggregate);

            var entity = await context.StockItems
                .Include(x => x.Units)
                .SingleAsync(x => x.Id == aggregate.StockItemId, cancellationToken);

            entity.Units = [.. aggregate.Units.Select(u => new StockUnitEntity
            {
                Id = u.Id,
                WarehouseId = u.WarehouseId,
                SerialNumber = u.SerialNumber.Value,
                IsLocked = u.IsLocked,
                LockedUntil = u.LockedUntil
            })];

            context.StockItems.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
        }

    }
}
