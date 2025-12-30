namespace Stock.Infrastructure.StockItems.StockUnits.CostSnapshot
{
    public class CostSnapshotEntity
    {
        public Guid Id { get; set; }

        public Guid? StockUnitId { get; set; }

        public StockUnitEntity? Unit { get; set; }

        public string? BatchId { get; set; }

        public decimal CostWithoutTax { get; set; }

        public decimal CostWithTax { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public required string SourceDocument { get; set; }
    }
}
