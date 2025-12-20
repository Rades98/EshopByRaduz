using Grpc.Core;

namespace Stock.Grpc.Services
{
    public class PriceInfoService : PriceInfo.PriceInfoBase
    {
        override public Task<PriceInfoReply> GetPrice(PriceInfoRequest request, ServerCallContext context)
        {
            var price = request.ProductId switch
            {
                "ProductA" => 19.99,
                "ProductB" => 29.99,
                "ProductC" => 39.99,
                _ => 0.0
            };

            var response = new PriceInfoReply
            {
                Price = price
            };
            return Task.FromResult(response);
        }
    }
}
