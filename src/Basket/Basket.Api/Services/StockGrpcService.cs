using Grpc.Net.Client;
using Stock.Grpc;

namespace Basket.Api.Services
{
    internal class StockGrpcService(IConfiguration configuration)
    {
        private StockService.StockServiceClient _client = new(GrpcChannel.ForAddress(configuration["services:stock-grpc:http:0"]!, new GrpcChannelOptions
        {
            HttpHandler = new HttpClientHandler
            {
                AllowAutoRedirect = true
            }
        }));

        public async Task<StockCountReply> GetStockCountAsync(StockCountRequest request, CancellationToken cancellationToken)
        {
            return await _client.GetStockCountAsync(request, cancellationToken: cancellationToken);
        }
    }
}
