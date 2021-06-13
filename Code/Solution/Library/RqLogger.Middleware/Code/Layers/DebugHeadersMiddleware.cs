using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RqLogger.Middleware.Layers
{
	public class DebugHeadersMiddleware
	{
		private readonly ILogger<DebugHeadersMiddleware> _Logger;

		private readonly RequestDelegate _Next;

		public DebugHeadersMiddleware(RequestDelegate next, ILogger<DebugHeadersMiddleware> logger)
		{
			_Next = next;
			_Logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			Contract.Assert(context is { Request: { } });

			if (!context.Request.Path.StartsWithSegments("/info", StringComparison.OrdinalIgnoreCase))
			{
				string path = $"{context.Request.Method} {context.Request.Path}";

				if (context.User != null && (context.User.Identity?.IsAuthenticated ?? false))
				{
					string userId = context.User.Identities.First().Label ?? "Anonymous";
					_Logger.LogInformation("Request {Path} | Authenticated User: {UserId}", path, userId);

					// context.Response.Headers.TryAdd(Header.AuthenticatedUser, userId);
				}
				else
					_Logger.LogDebug("Request {Path} | Unauthenticated", path);
			}

			await _Next(context);
		}
	}
}