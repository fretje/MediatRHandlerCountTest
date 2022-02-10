using MediatR;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<HandlerCounter>();

builder.Services.AddMediatR(typeof(Program));

WebApplication? app = builder.Build();

app.MapGet("/somethingHappened", (IPublisher publisher, HandlerCounter handlerCounter) =>
{
    publisher.Publish(new EventNotification<SomethingHappenedEvent>(new SomethingHappenedEvent()));
    return handlerCounter;
});

app.MapGet("/simple", (IPublisher publisher, HandlerCounter handlerCounter) =>
{
    publisher.Publish(new SimpleNotification());
    return handlerCounter;
});

app.MapGet("/both", (IPublisher publisher, HandlerCounter handlerCounter) =>
{
    publisher.Publish(new EventNotification<SomethingHappenedEvent>(new SomethingHappenedEvent()));
    publisher.Publish(new SimpleNotification());
    return handlerCounter;
});

app.Run();

public partial class Program { }

public abstract class DomainEvent
{
}

public class SomethingHappenedEvent : DomainEvent
{
}

public class EventNotification<TDomainEvent> : INotification
    where TDomainEvent : DomainEvent
{
    public EventNotification(TDomainEvent domainEvent) => DomainEvent = domainEvent;

    public TDomainEvent DomainEvent { get; }
}

public class SimpleNotification : INotification
{
}

public class SomethingHappenedEventHandler : INotificationHandler<EventNotification<SomethingHappenedEvent>>
{
    private readonly HandlerCounter _handlerCounter;

    public SomethingHappenedEventHandler(HandlerCounter handlerCounter) => _handlerCounter = handlerCounter;

    public Task Handle(EventNotification<SomethingHappenedEvent> notification, CancellationToken cancellationToken)
    {
        _handlerCounter.SomethingHappenedCount++;
        return Task.CompletedTask;
    }
}

public class SimpleNotificationEventHandler : INotificationHandler<SimpleNotification>
{
    private readonly HandlerCounter _handlerCounter;

    public SimpleNotificationEventHandler(HandlerCounter handlerCounter) => _handlerCounter = handlerCounter;

    public Task Handle(SimpleNotification notification, CancellationToken cancellationToken)
    {
        _handlerCounter.SimpleCount++;
        return Task.CompletedTask;
    }
}

public class GenericEventHandler<TNotification> : INotificationHandler<TNotification>
    where TNotification : INotification
{
    private readonly HandlerCounter _handlerCounter;

    public GenericEventHandler(HandlerCounter handlerCounter) => _handlerCounter = handlerCounter;

    public Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        _handlerCounter.GenericCount++;
        return Task.CompletedTask;
    }
}

public class HandlerCounter
{
    public int SomethingHappenedCount { get; set; }
    public int SimpleCount { get; set; }
    public int GenericCount { get; set; }
}