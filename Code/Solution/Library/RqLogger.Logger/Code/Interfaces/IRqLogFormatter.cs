using RqLogger.Logger.Middleware;

namespace RqLogger.Logger.Interfaces
{
	public interface IRqLogFormatter
	{
		string Format(RqLoggerManager manager);
	}
}