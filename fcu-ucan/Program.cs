using System;
using System.Threading.Tasks;
using fcu_ucan.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace fcu_ucan
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo
                .File("Logs/fcu-ucan.txt", shared: true, rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 512 * 1024 * 1024, rollOnFileSizeLimit: true)
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                var host = CreateHostBuilder(args).Build();
                await DataSeeder.Initialize(host);
                await host.RunAsync();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
