using MediatR;

namespace Mediator.Request.Command;

/// <summary>
/// command interface for CQRS needs
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "CQRS")]
public interface ICommand : IRequest
{
}

/// <summary>
/// command interface for CQRS needs
/// </summary>
/// <typeparam name="T">Type of response</typeparam>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "CQRS")]
public interface ICommand<out T> : IRequest<T>, ICommand
{
}
