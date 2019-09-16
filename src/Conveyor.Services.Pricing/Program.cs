using Convey;
using Convey.Discovery.Consul;
using Convey.LoadBalancing.Fabio;
using Convey.Tracing.Jaeger;
using Convey.WebApi;
using Conveyor.Services.Pricing.DTO;
using Conveyor.Services.Pricing.Queries;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Conveyor.Services.Pricing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddOpenTracing()
                    .AddConvey()
                    .AddConsul()
                    .AddFabio()
                    .AddJaeger()
                    .AddWebApi()
                    .Build())
                .Configure(app => app
                    .UseConsul()
                    .UseJaeger()
                    .UseEndpoints(endpoints => endpoints
                        .Get("ping", ctx => ctx.Response.WriteAsync("pong"))
                        .Get<GetOrderPricing>("pricing/{orderId}/orders", (query, ctx) =>
                        {
                            var json = JsonConvert.SerializeObject(new PricingDto
                            {
                                OrderId = query.OrderId,
                                TotalAmount = 20.50m
                            });

                            return ctx.Response.WriteAsync(json);
                        })));
    }
}
