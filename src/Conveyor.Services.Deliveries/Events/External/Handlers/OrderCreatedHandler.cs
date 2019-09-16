using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Conveyor.Services.Deliveries.RabbitMQ;
using Microsoft.Extensions.Logging;

namespace Conveyor.Services.Deliveries.Events.External.Handlers
{
    public class OrderCreatedHandler : IEventHandler<OrderCreated>
    {
        private readonly IBusPublisher _publisher;
        private readonly ILogger<OrderCreatedHandler> _logger;

        public OrderCreatedHandler(IBusPublisher publisher, ILogger<OrderCreatedHandler> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public Task HandleAsync(OrderCreated @event)
        {
            _logger.LogInformation($"Received a 'order created' event with id: {@event.OrderId}");
            return _publisher.PublishAsync(new DeliveryStarted(Guid.NewGuid()), new CorrelationContext());
        }
    }
}
