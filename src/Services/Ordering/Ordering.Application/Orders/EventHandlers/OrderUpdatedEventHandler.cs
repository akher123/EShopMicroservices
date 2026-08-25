namespace Ordering.Application.Orders.EventHandlers;

internal class OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger) : INotificationHandler<OrderUpdateEvent>
{
    public Task Handle(OrderUpdateEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event Handled:{DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}