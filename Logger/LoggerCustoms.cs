using NLog;

namespace nlogEx.Logger;

public static class LoggerCustoms
{
	public static void UpdateCorrelationIdInLogger(string correlationId)
	{
		var logger = LogManager.GetCurrentClassLogger();

		//if correlationId is null, send null to log
		logger.PushScopeProperty("correlationId", correlationId);
	}
}