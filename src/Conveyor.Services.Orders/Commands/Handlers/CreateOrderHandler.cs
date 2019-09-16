using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.MessageBrokers;
using Convey.Persistence.MongoDB;
using Conveyor.Services.Orders.Clients;
using Conveyor.Services.Orders.Domain;
using Conveyor.Services.Orders.Events;
using Conveyor.Services.Orders.RabbitMQ;
using Microsoft.EntityFrameworkCore.Design;

namespace Conveyor.Services.Orders.Commands.Handlers
{
    public class CreateOrderHandler : ICommandHandler<CreateOrder>
    {
        private readonly IMongoRepository<Order, Guid> _repository;
        private readonly IBusPublisher _publisher;
        private readonly IPricingServiceClient _pricingServiceClient;

        public CreateOrderHandler(IMongoRepository<Order, Guid> repository, IBusPublisher publisher,
            IPricingServiceClient pricingServiceClient)
        {
            _repository = repository;
            _publisher = publisher;
            _pricingServiceClient = pricingServiceClient;
        }

        public async Task HandleAsync(CreateOrder command)
        {
            var isExisting = await _repository.ExistsAsync(o => o.Id == command.Id);

            if (isExisting)
            {
                throw new OperationException("Order with given id already exists!");
            }

            var dto = await _pricingServiceClient.GetOrderPricingAsync(command.Id);
            var order = new Order(command.Id, command.CustomerId, dto.TotalAmount);

            await _repository.AddAsync(order);
            await _publisher.PublishAsync(new OrderCreated(order.Id), new CorrelationContext());
        }
    }
}
