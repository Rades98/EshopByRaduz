using Pricing.Domain.Pricing;

namespace Pricing.App.Pricing.GetPricesForProducts
{
    public sealed record GetPricesForProductsResponse(IReadOnlyList<PricePerItemResult> Items, MoneyValueObject Total);

    public sealed record PricePerItemResult(string Sku, string VariantId, MoneyValueObject UnitPrice, MoneyValueObject TotalPrice);
}
