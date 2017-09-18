using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Boardz.Id
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("hosting.json", optional: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();

            var host = new WebHostBuilder()
                .UseApplicationInsights()
                .CaptureStartupErrors(true)
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any, 443, cfg =>
                    {
                        cfg.UseHttps(Path.Combine(Directory.GetCurrentDirectory(), "Config", "certificate.pfx"), config.GetValue<string>("HostingCertPassword"));
                    });
                   
                })
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
