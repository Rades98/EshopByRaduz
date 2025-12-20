using Mediator.Request.Command;

namespace Mediator.Bus;

public interface ITaskBus
{
    ValueTask EnqueueAsync<T, TRes>(T request, TaskMetadata? metadata)
        where T : class, ICommand<TRes>;
}
