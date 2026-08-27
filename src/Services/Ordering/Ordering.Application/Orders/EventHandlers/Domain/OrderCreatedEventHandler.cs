using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers.Domain;

internal class OrderCreatedEventHandler(IPublishEndpoint publishEndpoint,IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger) : INotificationHandler<OrderCreateEvent>
{
    public async Task Handle(OrderCreateEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event Handled:{DomainEvent}", domainEvent.GetType().Name);
        if (await featureManager.IsEnabledAsync("OrderFullfilment"))
        {
            var orderCreatedInegrationEvent = domainEvent.Order.ToOrderDto();
            await publishEndpoint.Publish(orderCreatedInegrationEvent);
        }
    }
}
