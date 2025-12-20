using MediatR;

namespace Mediator.Request.Query;

public interface IPaginatedQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, PaginatedResult<TResult>>
    where TQuery : class, IPaginatedQuery<TResult>
{
}
