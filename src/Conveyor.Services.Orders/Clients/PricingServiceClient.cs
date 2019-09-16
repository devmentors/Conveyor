using System;
using System.Threading.Tasks;
using Convey.HTTP;
using Conveyor.Services.Orders.DTO;

namespace Conveyor.Services.Orders.Clients
{
    public class PricingServiceClient : IPricingServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;

        public PricingServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["pricing"];
        }

        public Task<PricingDto> GetOrderPricingAsync(Guid orderId)
            => _httpClient.GetAsync<PricingDto>($"{_url}/pricing/{orderId}/orders");
    }
}
