using MediatR;

namespace Mediator.Request.Query;

/// <summary>
/// Query handler
/// </summary>
/// <typeparam name="TQuery">query type</typeparam>
/// <typeparam name="TResponse">response type</typeparam>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : class, IQuery<TResponse>
{
}
