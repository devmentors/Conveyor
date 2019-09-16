using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Conveyor.Services.Orders.Events.External
{
    [MessageNamespace("deliveries")]
    public class DeliveryStarted : IEvent
    {
        public Guid Id { get; }

        public DeliveryStarted(Guid id)
        {
            Id = id;
        }
    }
}
