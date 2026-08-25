namespace Ordering.Application.Orders.EventHandlers;

internal class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger) : INotificationHandler<OrderCreateEvent>
{
    public Task Handle(OrderCreateEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event Handled:{DomainEvent}",notification.GetType().Name);
        return Task.CompletedTask;
    }
}
