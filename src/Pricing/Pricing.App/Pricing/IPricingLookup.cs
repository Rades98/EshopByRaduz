namespace Pricing.App.Pricing
{
    public interface IPricingLookup
    {
        Task<Guid> GetPriceGroupIdforProduct(string sku, string variantId, CancellationToken cancellationToken);
    }
}
