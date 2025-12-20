using MediatR;

namespace Mediator.Request.Query;

public interface IPaginatedQuery<T> : IRequest<PaginatedResult<T>>
{
    public int PageNumber { get; init; }

    public int Size { get; init; }
}
