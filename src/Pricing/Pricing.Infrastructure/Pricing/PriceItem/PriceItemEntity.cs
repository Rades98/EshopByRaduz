using DomainContracts;
using Pricing.Infrastructure.Currency;
using Pricing.Infrastructure.Pricing.PriceGroup;

namespace Pricing.Infrastructure.Pricing.PriceItem
{
    public class PriceItemEntity
    {
        public Guid Id { get; set; }

        public Guid PriceGroupId { get; set; }

        public PriceGroupEntity Group { get; set; } = null!;

        public decimal Price { get; set; }

        public required string CurrencyCode { get; set; }

        public CurrencyEntity Currency { get; set; } = null!;

        public PriceType PriceType { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }
    }
}
