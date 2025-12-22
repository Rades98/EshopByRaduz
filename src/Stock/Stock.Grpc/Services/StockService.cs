using Grpc.Core;
using MediatR;
using Stock.App.StockItems.AssignStockItemsToOrder;
using Stock.App.StockItems.GetStockCount;
using Stock.App.StockItems.ReserveStockItems;

namespace Stock.Grpc.Services
{
    internal sealed class StockService(IMediator mediator) : Grpc.StockService.StockServiceBase
    {
        public override async Task<StockCountReply> GetStockCount(StockCountRequest request, ServerCallContext context)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            var commandRequest = request.Items.Select(x => new StockItemCountRequest(x.Sku, x.Variation)).ToList().AsReadOnly();

            var res = await mediator.Send(new GetStockCountQuery(commandRequest), context.CancellationToken);

            return new StockCountReply()
            {
                Stocks = { res.Select(x => new StockItemStock
                {
                    Item = new()
                    {
                        Sku = x.Sku,
                        Variation = x.VariantId,
                    },
                    Available = x.Amount,
                }) }
            };
        }

        public override async Task<ReserveStockResponse> ReserveStock(ReserveStockRequest request, ServerCallContext context)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            var commandRequest = new ReserveStockItemsRequest(
                request.Items.Select(x => new ReserveStockItemRequest(x.Item.Sku, x.Item.Variation, x.Quantity)).ToList().AsReadOnly(),
                Guid.Parse(request.CheckoutReference));

            var res = await mediator.Send<bool>(new ReserveStockItemsCommand(commandRequest), context.CancellationToken);

            if (res)
            {
                return new ReserveStockResponse()
                {
                    Success = true,
                    ReservedItems =
                    {
                        request.Items.Select(x => new StockItemWithQuantity
                        {
                           Item = new()
                            {
                                Sku = x.Item.Sku,
                                Variation = x.Item.Variation,
                            },
                            Quantity = x.Quantity
                        })
                    }
                };
            }
            else
            {
                return new ReserveStockResponse()
                {
                    Success = false,
                    Error = "Failed to reserve stock items."
                };
            }
        }

        public override async Task<AssignToOrderResponse> AssignToOrder(AssignToOrderRequest request, ServerCallContext context)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            var res = await mediator.Send<AssignStockItemsToOrderResponse>(new AssignStockItemsToOrderCommand(Guid.Parse(request.CheckoutReference), Guid.Parse(request.OrderReference)), context.CancellationToken);

            var response = new AssignToOrderResponse
            {
                Success = res.Success
            };

            if (res.Items != null)
            {
                foreach (var item in res.Items)
                {
                    var stockItemUnits = new StockItemUnits
                    {
                        Item = new()
                        {
                            Sku = item.Sku,
                            Variation = item.VariantId
                        }
                    };

                    stockItemUnits.Units.AddRange(item.SerialNumbers.Select(u => new StockUnit
                    {
                        SerialNumber = u
                    }));

                    response.AssignedUnits.Add(stockItemUnits);
                }
            }

            return response;
        }
    }
}