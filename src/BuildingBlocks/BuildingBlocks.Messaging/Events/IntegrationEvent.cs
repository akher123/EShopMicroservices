namespace BuildingBlocks.Messaging.Events;

public record IntegrationEvent
{
    public Guid Id=>Guid.NewGuid();
    public DateTime OccurredOn{ get; set; }

    public string EventType=>GetType().AssemblyQualifiedName;
}
