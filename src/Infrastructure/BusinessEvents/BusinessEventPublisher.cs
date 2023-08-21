
using StorageSpike.Application.Core;

namespace Infrastructure.BusinessEvents;

public class BusinessEventPublisher : IBusinessEventPublisher
{
    private readonly IEventSink _eventSink;

    public BusinessEventPublisher(IEventSink eventSink)
        => _eventSink = eventSink;
}

public record BusinessEvent(int Tenant, int EntryPoint);

public interface IEventSink
{
    Task PublishEventAsync(BusinessEvent businessEvent);
}
