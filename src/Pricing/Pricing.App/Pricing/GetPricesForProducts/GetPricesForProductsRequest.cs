using DomainContracts;
using System.Collections.ObjectModel;

namespace Pricing.App.Pricing.GetPricesForProducts
{
    public sealed record GetPricesForProductsRequest(PriceType PriceType, string CurrencyCode, ReadOnlyCollection<GetPriceForProductsRequest> Items);

    public sealed record GetPriceForProductsRequest(string Sku, string VariantId, int Quantity);
}
