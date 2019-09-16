using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Conveyor.Services.Deliveries.Events.External
{
    [MessageNamespace("orders")]
    public class OrderCreated : IEvent
    {
        public Guid Id { get; }

        public OrderCreated(Guid id)
        {
            Id = id;
        }
    }
}
