using DomainObjects;
using Stock.Domain.StockItems.StockUnits;

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

        public int ReservedCount() => _units.Count(u => !u.IsAvailable());

        public bool TryLockUnits(int count, DateTime until, Guid checkoutReference)
        {
            var available = _units.Where(u => u.IsAvailable()).Take(count).ToList();
            if (available.Count < count)
            {
                return false;
            }

            foreach (var unit in available) unit.Reserve(until, checkoutReference);
            return true;
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
                    "StockFill"
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
