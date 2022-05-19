using Autofac;
using Microsoft.Extensions.Hosting;
using nlogEx.Services.EndpointFileCheckingService;

namespace nlogEx.IOC
{
	public class WorkingModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			RegisterBackgroundServices(builder);
		}

		private void RegisterBackgroundServices(ContainerBuilder builder)
		{
			builder.RegisterType<EndpointFileCheckingService>().As<IHostedService>().SingleInstance();
		}
	}
}