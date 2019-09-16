using System;
using Convey.CQRS.Events;

namespace Conveyor.Services.Orders.Events
{
    public class OrderCreated : IEvent
    {
        public Guid Id { get; }

        public OrderCreated(Guid id)
        {
            Id = id;
        }
    }
}
