using System;
using System.Threading.Tasks;
using Conveyor.Services.Orders.DTO;

namespace Conveyor.Services.Orders.Clients
{
    public interface IPricingServiceClient
    {
        Task<PricingDto> GetOrderPricingAsync(Guid orderId);
    }
}
