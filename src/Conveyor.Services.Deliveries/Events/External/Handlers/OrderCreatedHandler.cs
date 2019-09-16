using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.MessageBrokers.CQRS;
using Conveyor.Services.Deliveries.RabbitMQ;

namespace Conveyor.Services.Deliveries.Events.External.Handlers
{
    public class OrderCreatedHandler : IEventHandler<OrderCreated>
    {
        private readonly IBusPublisher _publisher;

        public OrderCreatedHandler(IBusPublisher publisher)
        {
            _publisher = publisher;
        }

        public Task HandleAsync(OrderCreated @event)
            => _publisher.PublishAsync(new DeliveryStarted(Guid.NewGuid()), new CorrelationContext());
    }
}
