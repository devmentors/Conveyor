﻿using System;
using Convey;
using Convey.CQRS.Commands;
using Convey.Discovery.Consul;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.Logging;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.MongoDB;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Conveyor.Services.Orders.Commands;
using Conveyor.Services.Orders.Domain;
using Conveyor.Services.Orders.RabbitMQ;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
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
                    .AddInMemoryCommandDispatcher()
                    .AddRabbitMq<CorrelationContext>(plugins: p => p.RegisterJaeger())
                    .AddMetrics()
                    .AddWebApi()
                    .Build())
                .Configure(app => app
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Post<CreateOrder>("orders",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"resources/{cmd.Id}")))
                    .UseConsul()
                    .UseJaeger()
                    .UseMetrics()
                    .UseErrorHandler()
                    .UseRabbitMq())
                .UseLogging();
    }
}
