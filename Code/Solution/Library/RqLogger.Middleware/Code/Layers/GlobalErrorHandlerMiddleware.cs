using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using RqLogger.Middleware.Mapper;
using RqLogger.Middleware.ViewModels;

namespace RqLogger.Middleware.Layers
{
	public class GlobalErrorHandlerMiddleware
	{
		private readonly ILogger<GlobalErrorHandlerMiddleware> _Logger;

		private readonly RequestDelegate _Next;

		public GlobalErrorHandlerMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlerMiddleware> logger)
		{
			_Logger = logger;
			_Next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			Contract.Assume(httpContext != null);

			try
			{
				await _Next(httpContext);
			}
#pragma warning disable CA1031
			catch (Exception ex)
			{
				await HandleException(httpContext, ex);
			}
#pragma warning restore CA1031
		}

		private Task HandleException(HttpContext context, Exception exception)
		{
			ErrorDetails error = ErrorMapper.MapErrorFrom(exception, context);
			string serializedError = error.ToJsonString();

			// Log the error with trace
			_Logger.LogError(exception, "{Error}\n{Detail}", error.Error, serializedError);

			// context.Response.Clear();
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			return context.Response.WriteAsync(serializedError);
		}
	}
}