using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace WindowsServiceCore
{
	public class ApplicationHost
    {
        private IWebHost _webHost;

        public void Start(bool launchedFromConsole, string[] args)
        {
	        if (!launchedFromConsole)
	        {
		        var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
		        var pathToContentRoot = Path.GetDirectoryName(pathToExe);
		        Directory.SetCurrentDirectory(pathToContentRoot);
	        }

            IWebHostBuilder webHostBuilder = CreateWebHostBuilder(args);
            _webHost = webHostBuilder.Build();

            _webHost.Start();

            // print information to console
            if (launchedFromConsole)
            {
                var serverAddresses = _webHost.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses;
                foreach (var address in serverAddresses ?? Array.Empty<string>())
                {
                    Console.WriteLine($"Listening on: {address}");
                    Console.WriteLine("Press Ctrl+C to end the application.");
                }
            }
        }

        public void Stop()
        {
            _webHost?.Dispose();
        }

        private IWebHostBuilder CreateWebHostBuilder(string[] args) =>
	        WebHost.CreateDefaultBuilder(args)
		        .ConfigureLogging((hostingContext, logging) =>
		        {
			        logging.AddEventLog();
		        })
		        .ConfigureAppConfiguration((context, config) =>
		        {
			        // Configure the app here.
		        })
		        .UseUrls("http://+:8000")
		        .UseStartup<Startup>();
    }
}
