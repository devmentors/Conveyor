using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Conveyor.Services.Deliveries.Events.External
{
    [MessageNamespace("orders")]
    public class OrderCreated : IEvent
    {
        public Guid OrderId { get; }

        public OrderCreated(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
