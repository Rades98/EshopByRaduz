using Pricing.Infrastructure.Pricing.PriceItem;

namespace Pricing.Infrastructure.Pricing.PriceGroup
{
    public class PriceGroupEntity
    {
        public Guid Id { get; set; }

        public required string Sku { get; set; } = null!;

        public required string VariantId { get; set; } = null!;

        public ICollection<PriceItemEntity> Items { get; } = [];
    }
}
