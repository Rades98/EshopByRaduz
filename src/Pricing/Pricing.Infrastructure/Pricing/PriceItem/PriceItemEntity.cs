using DomainContracts;
using Pricing.Infrastructure.Currency;

namespace Pricing.Infrastructure.Pricing.PriceItem
{
    public class PriceItemEntity
    {
        public Guid Id { get; set; }

        public string Sku { get; set; } = null!;

        public string VariantId { get; set; } = null!;

        public decimal Price { get; set; }

        public string? CurrencyCode { get; set; }

        public CurrencyEntity Currency { get; set; } = null!;

        public PriceType PriceType { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public bool IsApplicable { get; set; }
    }
}
