using Stock.Infrastructure.Warehouses;

namespace Stock.Infrastructure.StockItems.StockUnits
{
    public class StockUnitEntity
    {
        public Guid Id { get; set; }

        public Guid StockItemId { get; set; }

        public StockItemEntity? StockItem { get; set; }

        public required string SerialNumber { get; set; }

        public bool IsLocked { get; set; }

        public Guid? CheckoutReference { get; set; }

        public bool IsSold { get; set; }

        public Guid? OrderReference { get; set; }

        public DateTime? LockedUntil { get; set; }

        public Guid? WarehouseId { get; set; }

        public WarehouseEntity? Warehouse { get; set; }
    }
}
