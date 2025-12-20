using MediatR;
using Microsoft.Extensions.Logging;

namespace Mediator.Request.Transaction;

public sealed class TransactionPipeline<TRequest, TResponse>(ITransactionUnitOfWork uow, ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITransactionedRequest
    where TResponse : ITransactionedRequestResponse, new()
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await uow.BeginTransactionAsync(cancellationToken);

        TResponse res = new()
        {
            Failed = true
        };

        try
        {
            res = await next();
            await uow.CommitTransactionAsync(cancellationToken);

            res.Failed = false;

            return res;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occured while trying to manage data within transaction");

            await uow.RollbackTransactionAsync(cancellationToken);

            return res;
        }
    }
}
