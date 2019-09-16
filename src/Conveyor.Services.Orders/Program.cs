using System;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.Logging;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.MongoDB;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Conveyor.Services.Orders.Commands;
using Conveyor.Services.Orders.Domain;
using Conveyor.Services.Orders.DTO;
using Conveyor.Services.Orders.Events.External;
using Conveyor.Services.Orders.Queries;
using Conveyor.Services.Orders.RabbitMQ;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Conveyor.Services.Orders
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
                    .AddServices()
                    .AddHttpClient()
                    .AddConsul()
                    .AddFabio()
                    .AddJaeger()
                    .AddMongo()
                    .AddMongoRepository<Order, Guid>("orders")
                    .AddCommandHandlers()
                    .AddEventHandlers()
                    .AddQueryHandlers()
                    .AddInMemoryCommandDispatcher()
                    .AddInMemoryEventDispatcher()
                    .AddInMemoryQueryDispatcher()
                    .AddRabbitMq<CorrelationContext>(plugins: p => p.RegisterJaeger())
                    .AddMetrics()
                    .AddWebApi()
                    .Build())
                .Configure(app => app
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync("Orders Service"))
                        .Get("ping", ctx => ctx.Response.WriteAsync("pong"))
                        .Get<GetOrder, OrderDto>("orders/{orderId}")
                        .Post<CreateOrder>("orders",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"orders/{cmd.OrderId}")))
                    .UseConsul()
                    .UseJaeger()
                    .UseMetrics()
                    .UseErrorHandler()
                    .UseMvc()
                    .UseRabbitMq()
                    .SubscribeEvent<DeliveryStarted>())
                .UseLogging()
                .UseLogging();
    }
}
