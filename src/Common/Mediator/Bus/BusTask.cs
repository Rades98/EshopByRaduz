using MediatR;

namespace Mediator.Bus;

internal sealed record BusTask(Func<IMediator, Task> Action, TaskMetadata? TaskMetadata);
