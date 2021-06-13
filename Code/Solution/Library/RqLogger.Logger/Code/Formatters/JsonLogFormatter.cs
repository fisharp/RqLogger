using System.Text.Json;
using System.Text.Json.Serialization;

using RqLogger.Logger.Interfaces;
using RqLogger.Logger.Middleware;

namespace RqLogger.Logger.Formatters
{
	public class JsonLogFormatter : IRqLogFormatter
	{
		private readonly JsonSerializerOptions _SerializerOptions = new JsonSerializerOptions()
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
		};

		public string Format(RqLoggerManager manager)
		{
			return JsonSerializer.Serialize(manager, _SerializerOptions);
		}
	}
}