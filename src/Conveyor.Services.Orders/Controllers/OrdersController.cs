using System.Threading.Tasks;
using Conveyor.Services.Orders.Commands;
using Conveyor.Services.Orders.DTO;
using Conveyor.Services.Orders.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Conveyor.Services.Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDto>> Get([FromRoute] GetOrder query)
        {
            await Task.CompletedTask;
            var order = new OrderDto {Id = query.OrderId};
            if (order is null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateOrder command)
        {
            await Task.CompletedTask;
            return Created($"orders/{command.OrderId}", null);
        }
    }
}
