using Topshelf;
using Topshelf.Hosts;

namespace WindowsServiceCore
{
	class Program
	{
		public static void Main(string[] args)
		{
			HostFactory.Run(x =>
			{
				x.Service<ApplicationHost>(sc =>
				{
					sc.ConstructUsing(() => new ApplicationHost());

					// the start and stop methods for the service
					sc.WhenStarted((svc, control) =>
					{
						svc.Start(control is ConsoleRunHost, args);
						return true;
					});
					sc.WhenStopped(s => s.Stop());
					sc.WhenShutdown(s => s.Stop());
				});

				x.UseNLog();
				x.RunAsLocalSystem();
				x.StartAutomatically();

				x.SetServiceName("Test Core");
				x.SetDisplayName("Test ASP.NET Core Service");
				x.SetDescription("Test ASP.NET Core as Windows Service.");
			});
		}
	}
}
