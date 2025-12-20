using Grpc.Core;
using MediatR;
using Stock.App.StockItems.StockUnits;

namespace Stock.Grpc.Services
{
    internal class StockCountService(IMediator mediator) : StockCount.StockCountBase
    {
        public override async Task<StockCountReply> GetStockCount(StockCountRequest request, ServerCallContext context)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            var res = await mediator.Send(new GetStockCountQuery(request.SKU, request.VariationId), context.CancellationToken);

            return new StockCountReply()
            {
                Available = res
            };
        }
    }
}
