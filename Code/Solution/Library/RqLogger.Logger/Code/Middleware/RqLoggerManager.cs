using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Logging;

namespace RqLogger.Logger.Middleware
{
	public class RqLoggerManager : IDisposable
	{
		private readonly Stopwatch _Watch;

		private readonly ConcurrentQueue<LogEntry> _RequestLogs;

		public RqLoggerManager()
		{
			RequestId = Guid.NewGuid();
			_Watch = Stopwatch.StartNew();
			_RequestLogs = new ConcurrentQueue<LogEntry>();
		}

		public async void AppendLog(int eventId, LogLevel level, string text, Exception exception)
		{
			long elapsed = _Watch.ElapsedMilliseconds;
			// _Watch.Restart();
			_RequestLogs.Enqueue(new LogEntry
			{
				RequestId = RequestId,
				ElapsedTime = elapsed,
				EventId = eventId,
				Level = level,
				Message = text,
				Exception = exception
			});
		}

		/// <summary>
		/// Max Level of internal logs.
		/// </summary>
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public LogLevel Level => _RequestLogs.ToList().Max(l => l.Level);

		public Guid RequestId { get; }

		public ICollection<LogEntry> Logs => _RequestLogs.ToArray();

		public override string ToString()
		{
			return $"{RequestId.ToString()} - {_RequestLogs.Count} log entries";
		}

		void IDisposable.Dispose()
		{
			_Watch.Stop();
			_RequestLogs.Clear();
		}
	}
}