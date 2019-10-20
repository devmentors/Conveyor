using Convey.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Conveyor.Services.Orders
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services => services
                        .Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; })
                        .AddMvcCore()
                        .AddNewtonsoftJson())
                    .Configure(app => app
                        .UseRouting()
                        .UseEndpoints(r => r.MapControllers()))
                    .UseLogging();
            });
    }
}
