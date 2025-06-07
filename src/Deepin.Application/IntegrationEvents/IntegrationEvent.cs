namespace Deepin.Application.IntegrationEvents;

public abstract record IntegrationEvent
{
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
    }
    public IntegrationEvent(Guid id, DateTimeOffset creationDateTimeOffset)
    {
        Id = id;
        CreatedAt = DateTimeOffset.UtcNow;
    }
    public Guid Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
}
