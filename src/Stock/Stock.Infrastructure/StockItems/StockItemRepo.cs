using Database.SQL;
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
                        res.Sku,
                        res.VariantId,
                        res.Units
                            .Where(x => !x.IsSold)
                            .Select(x =>
                            StockUnitModel.Rehydrate(
                                x.Id,
                                x.WarehouseId,
                                SerialNumberValueObject.Create(x.SerialNumber).Value!,
                                x.IsLocked,
                                x.CheckoutReference,
                                x.LockedUntil,
                                x.IsSold,
                                x.OrderReference))))
                .SingleAsync(cancellationToken)!;

        public Task SaveAsync(StockItemAggregate aggregate, CancellationToken cancellationToken)
            => context.UpdateWithinTransaction<StockItemEntity, StockDbContext>(async context =>
            {
                ArgumentNullException.ThrowIfNull(aggregate);

                var entity = await context.StockItems
                    .Include(x => x.Units)
                        .ThenInclude(u => u.Warehouse)
                    .SingleAsync(x => x.Id == aggregate.StockItemId, cancellationToken);

                var unitsToRemove = entity.Units
                    .Where(u => !aggregate.Units.Any(au => au.Id == u.Id))
                    .ToList();

                foreach (var u in unitsToRemove)
                {
                    context.StockUnits.Remove(u);
                }

                foreach (var existing in entity.Units)
                {
                    var updatedUnit = aggregate.Units.FirstOrDefault(u => u.Id == existing.Id);
                    if (updatedUnit != null)
                    {
                        existing.SerialNumber = updatedUnit.SerialNumber.Value;
                        existing.WarehouseId = updatedUnit.WarehouseId;

                        existing.IsLocked = updatedUnit.IsLocked;
                        existing.LockedUntil = updatedUnit.LockedUntil;
                        existing.CheckoutReference = updatedUnit.CheckoutReference;

                        existing.IsSold = updatedUnit.IsSold;
                        existing.OrderReference = updatedUnit.OrderReference;
                    }

                    context.StockUnits.Update(existing);
                }

                var existingIds = entity.Units.Select(u => u.Id).ToHashSet();
                foreach (var unit in aggregate.Units)
                {
                    if (!existingIds.Contains(unit.Id))
                    {
                        context.StockUnits.Add(new StockUnitEntity
                        {
                            Id = unit.Id,
                            StockItemId = entity.Id,
                            SerialNumber = unit.SerialNumber.Value,
                            WarehouseId = unit.WarehouseId,

                            IsLocked = unit.IsLocked,
                            LockedUntil = unit.LockedUntil,
                            CheckoutReference = unit.CheckoutReference,

                            IsSold = unit.IsSold,
                            OrderReference = unit.OrderReference,
                        });
                    }
                }
            }, cancellationToken);
    }
}
