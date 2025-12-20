using Mediator.Request.Command;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading.Channels;

namespace Mediator.Bus;

internal class TaskBus : ITaskBus
{
    private readonly IMediator _mediator;
    private readonly Channel<BusTask> _channel;
    private readonly ILogger<TaskBus> _logger;

    private static readonly Meter _busMeter = new("task.bus");

    private static readonly Counter<long> _received = _busMeter.CreateCounter<long>("received");
    private static readonly Counter<long> _completed = _busMeter.CreateCounter<long>("completed");
    private static readonly Counter<long> _failed = _busMeter.CreateCounter<long>("failed");
    private static readonly Histogram<double> _duration = _busMeter.CreateHistogram<double>("duration", "ms");

    public TaskBus(IMediator mediator, ILogger<TaskBus> logger)
    {
        _mediator = mediator;
        _channel = Channel.CreateUnbounded<BusTask>();
        _logger = logger;

        _busMeter.CreateObservableGauge("queue_length", () =>
            new Measurement<long>(_channel.Reader.Count)
        );

        _ = Task.Run(Process);
    }

    public ValueTask EnqueueAsync<T, TRes>(T request, TaskMetadata? metadata)
        where T : class, ICommand<TRes>
    {
        _received.Add(1);
        return _channel.Writer.WriteAsync(new((mediator) => mediator.Send<TRes>(request), metadata));
    }

    private async Task Process()
    {
        await foreach (var request in _channel.Reader.ReadAllAsync())
        {
            var sw = Stopwatch.StartNew();

            try
            {
                await request.Action.Invoke(_mediator);
                _completed.Add(1);
            }
            catch (Exception ex)
            {
                _failed.Add(1);
                _logger.LogError(ex, "An exception has occured while processing data");
            }
            finally
            {
                _duration.Record(sw.Elapsed.TotalMilliseconds);
            }
        }
    }
}
