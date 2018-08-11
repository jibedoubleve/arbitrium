using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Probel.Arbitrium
{
    public class Program
    {
        #region Methods

        public static IWebHost BuildWebHost(string[] args)
        {
            // use this to allow command line parameters in the config
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var hostUrl = configuration["hosturl"];
            if (string.IsNullOrEmpty(hostUrl))
            {
                hostUrl = "http://0.0.0.0:80";
            }

            var host = WebHost.CreateDefaultBuilder(args)
                              .UseUrls(hostUrl)
                              .UseStartup<Startup>()
                              .Build();

            return host;
        }

        public static void Main(string[] args) => BuildWebHost(args).Run();

        #endregion Methods
    }
}