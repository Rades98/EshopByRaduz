using InOutbox.Orchestrator;

namespace Stock.Infrastructure.StockItems.StockUnits.CostSnapshot.Buffer
{
    internal class CostSnapshotBufferEntity
    {
        public Guid Id { get; set; }

        public required string SKU { get; set; }

        public required string Variant { get; set; }

        public string? BatchId { get; set; }

        public decimal CostWithoutTax { get; set; }

        public decimal CostWithTax { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public required string SourceDocument { get; set; }

        public InOutboxStatus Status { get; set; }
    }
}
