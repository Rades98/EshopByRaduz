using DomainObjects;

namespace DomainContracts.Events.Pricing
{
    public record PriceItemAddedEvent(string Sku, string Variant, string PriceType, decimal Price, string Currency, string Source, DateTime ValidFrom, DateTime? ValidTo)
        : DomainEventVithValidity(ValidFrom, ValidTo);
}
