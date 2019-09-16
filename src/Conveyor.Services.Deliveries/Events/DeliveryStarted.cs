using System;
using Convey.CQRS.Events;

namespace Conveyor.Services.Deliveries.Events
{
    public class DeliveryStarted : IEvent
    {
        public Guid Id { get; }

        public DeliveryStarted(Guid id)
        {
            Id = id;
        }
    }
}
