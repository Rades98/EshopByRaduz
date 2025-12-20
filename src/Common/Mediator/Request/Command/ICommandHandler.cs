using MediatR;

namespace Mediator.Request.Command;

/// <summary>
/// Command handler
/// </summary>
/// <typeparam name="TCommand">Command type</typeparam>
/// <typeparam name="TResponse">response type</typeparam>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : class, ICommand<TResponse>
{
}
