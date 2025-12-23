using Grpc.Net.Client;
using Pricing.Grpc;

namespace Basket.Api.Services
{
    internal class PricingGrpcService(IConfiguration configuration)
    {
        private PricingService.PricingServiceClient _client = new(GrpcChannel.ForAddress(configuration["services:pricing-grpc:http:0"]!, new GrpcChannelOptions
        {
            HttpHandler = new HttpClientHandler
            {
                AllowAutoRedirect = true
            }
        }));

        public async Task<GetComputedPricesForProductsResponse> GetStockCountAsync(GetPricesRequest request, CancellationToken cancellationToken)
        {
            return await _client.GetPricesAsync(request, cancellationToken: cancellationToken);
        }
    }
}
