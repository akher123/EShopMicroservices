namespace Ordering.Domain.Events;

public record OrderUpdateEvent(Order Order) : IDomainEvent;

