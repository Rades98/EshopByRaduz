using DomainObjects;
using MediatR;
using Stock.Domain.StockItems.StockUnits;
using Stock.Domain.StockItems.StockUnits.Events;
using System.Collections.ObjectModel;

namespace Stock.Domain.StockItems
{
    public class StockItemAggregate(Guid stockItemId, string sku, string variantId) : AggregateRoot
    {
        public Guid StockItemId { get; } = stockItemId;

        public string Sku { get; } = sku;

        public string VariantId { get; } = variantId;

        private readonly List<StockUnitModel> _units = [];

        public IReadOnlyCollection<StockUnitModel> Units => _units;

        public static StockItemAggregate Rehydrate(Guid id, string sku, string variantId, IEnumerable<StockUnitModel> units)
        {
            var agg = new StockItemAggregate(id, sku, variantId);
            agg._units.AddRange(units);
            return agg;
        }

        public int GetAvailableCount() => _units.Count(u => u.IsAvailable());

        public ReadOnlyCollection<StockUnitModel> GetAssignedToOrder(Guid orderReference)
            => _units.Where(x => x.OrderReference == orderReference).ToList().AsReadOnly();

        public int ReservedCount() => _units.Count(u => !u.IsAvailable());

        public bool TryLockUnits(int count, DateTime until, Guid checkoutReference)
        {
            var available = _units.Where(u => u.IsAvailable()).Take(count).ToList();
            if (available.Count < count)
            {
                return false;
            }

            foreach (var unit in available)
            {
                if (!unit.Reserve(until, checkoutReference))
                {
                    return false;
                }

                AddDomainEvent(new StockUnitLockedEvent(
                    Sku,
                    VariantId,
                    "Stock"
                ));
            }

            return true;
        }

        public Result<Unit> TryAssignUnitsToOrder(Guid orderId, Guid checkoutReference)
        {
            var assignable = _units.Where(u => u.IsLocked && !u.IsSold && u.CheckoutReference == checkoutReference).ToList();

            foreach (var unit in assignable)
            {
                var res = unit.AssignToOrder(orderId, checkoutReference);
                if (!res.IsSuccess)
                {
                    return Result<Unit>.Failure(res.Error!);
                }

                AddDomainEvent(new StockUnitAssignedEvent(
                    Sku,
                    VariantId,
                    orderId,
                    "Stock"
                ));
            }

            return Result<Unit>.Success(Unit.Value);
        }


        public Result<StockUnitModel> TryAddUnit(Guid id, string serial, Guid warehouseId)
        {
            if (_units.Any(u => u.SerialNumber.Value == serial))
            {
                return Result<StockUnitModel>.Failure("STOCKUNIT_ERROR_SERIAL_IN_USE");
            }

            var unitToCreate = StockUnitModel.CreateNew(id, serial, warehouseId);

            if (unitToCreate.IsSuccess && unitToCreate.Value is StockUnitModel stockUnit)
            {
                _units.Add(stockUnit);

                AddDomainEvent(new StockUnitAddedEvent(
                    Sku,
                    VariantId,
                    "Stock"
                ));

                return Result<StockUnitModel>.Success(unitToCreate.Value!);
            }
            else
            {
                return Result<StockUnitModel>.Failure(unitToCreate.Error!);
            }
        }
    }
}
