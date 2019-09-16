using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Microsoft.Extensions.Logging;

namespace Conveyor.Services.Orders.Events.External.Handlers
{
    [MessageNamespace("deliveries")]
    public class DeliveryStartedHandler : IEventHandler<DeliveryStarted>
    {
        private readonly ILogger<DeliveryStartedHandler> _logger;

        public DeliveryStartedHandler(ILogger<DeliveryStartedHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(DeliveryStarted @event)
        {
            _logger.LogInformation($"Received delivery started event with id {@event.Id}");
        }
    }
}
