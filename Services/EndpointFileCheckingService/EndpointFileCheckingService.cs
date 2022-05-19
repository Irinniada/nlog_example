using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using nlogEx.Logger;

namespace nlogEx.Services.EndpointFileCheckingService
{
	public class EndpointFileCheckingService : BackgroundService
	{
		private readonly ILogger<EndpointFileCheckingService> _logger;

		public EndpointFileCheckingService(ILogger<EndpointFileCheckingService> logger)
		{
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				var correlationId = Guid.NewGuid().ToString("D");
				LoggerCustoms.UpdateCorrelationIdInLogger(correlationId);
                
				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                
				_logger.LogDebug("DEBUG Worker running at: {time}", DateTimeOffset.Now);
                
				await Task.Delay(1000, stoppingToken);

				//LogManager.Setup().ReloadConfiguration();
			}
		}
	}
}