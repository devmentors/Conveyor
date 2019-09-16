using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Conveyor.Services.Orders.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Conveyor.Services.Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public OrdersController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateOrder command)
        {
            await _commandDispatcher.SendAsync(command);
            return Created($"orders/{command.OrderId}", null);
        }
    }
}
