using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using Topshelf;

namespace WindowsServiceCore
{
	class Program
	{
		public static void Main(string[] args)
		{
			var isService = !(Debugger.IsAttached || args.Contains("--console"));

			if (isService)
			{
				var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
				var pathToContentRoot = Path.GetDirectoryName(pathToExe);
				Directory.SetCurrentDirectory(pathToContentRoot);
			}

			var builder = CreateWebHostBuilder(args.Where(arg => arg != "--console").ToArray());
			var host = builder.Build();

			if (isService)
			{
				HostFactory.Run(x =>
				{
					x.Service<CustomWebHostService>(sc =>
					{
						sc.ConstructUsing(() => new CustomWebHostService(host));

						// the start and stop methods for the service
						sc.WhenStarted(ServiceBase.Run);
						sc.WhenStopped(s => s.Stop());
					});

					x.RunAsLocalSystem();
					x.StartAutomatically();

					x.SetServiceName("Teste Core");
					x.SetDisplayName("Teste ASP.NET Core Service");
					x.SetDescription("Teste ASP.NET Core as Windows Service.");
				});
			}
			else
			{
				host.Run();
			}
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
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
