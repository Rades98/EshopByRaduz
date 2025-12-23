using DomainContracts;
using System.Collections.ObjectModel;

namespace Pricing.App.Pricing.UpdatePricesForProduct
{
    public sealed record UpdatePricesForProductRequest(string Sku, string Variant, ReadOnlyCollection<UpdatePricesForProductItemRequest> Items);

    public sealed record UpdatePricesForProductItemRequest(PriceType PriceType, decimal Price, string CurrencyCode, DateTime ValidFrom, DateTime? ValidTo = null);
}
