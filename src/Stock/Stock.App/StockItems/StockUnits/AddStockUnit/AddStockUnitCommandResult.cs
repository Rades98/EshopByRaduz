using Mediator.Request.Transaction;

namespace Stock.App.StockItems.StockUnits.AddStockUnit
{
    public class AddStockUnitCommandResult : ITransactionedRequestResponse
    {
        public bool Failed { get; set; }
    }
}
