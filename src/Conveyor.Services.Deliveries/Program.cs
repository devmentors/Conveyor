using Convey;
using Convey.CQRS.Events;
using Convey.Discovery.Consul;
using Convey.LoadBalancing.Fabio;
using Convey.Logging;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi;
using Conveyor.Services.Deliveries.Events.External;
using Conveyor.Services.Deliveries.RabbitMQ;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Conveyor.Services.Deliveries
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
                    .AddEventHandlers()
                    .AddInMemoryEventDispatcher()
                    .AddRabbitMq<CorrelationContext>(plugins: p => p.RegisterJaeger())
                    .AddMetrics()
                    .AddWebApi()
                    .Build())
                .Configure(app => app
                    .UseEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync("Deliveries Service")))
                    .UseJaeger()
                    .UseMetrics()
                    .UseErrorHandler()
                    .UseRabbitMq()
                    .SubscribeEvent<OrderCreated>())
                .UseLogging();
    }
}
