using Stock.Infrastructure.StockItems.StockUnits;

namespace Stock.Infrastructure.StockItems
{
    public class StockItemEntity
    {
        public Guid Id { get; set; }

        public required string Sku { get; set; }

        public required string VariantId { get; set; }

        public ICollection<StockUnitEntity> Units { get; } = [];
    }
}
