using MediatR;

namespace Mediator.Request.Query;

/// <summary>
/// query interface for CQRS needs
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "CQRS")]
public interface IQuery
{
}

/// <summary>
/// command interface for CQRS needs
/// </summary>
/// <typeparam name="T">Response type</typeparam>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "CQRS")]
public interface IQuery<out T> : IRequest<T>, IQuery
{
}
