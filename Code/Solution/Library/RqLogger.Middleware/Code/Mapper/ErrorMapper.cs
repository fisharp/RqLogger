using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Text.Json;

using Microsoft.AspNetCore.Http;

using RqLogger.Middleware.ViewModels;

namespace RqLogger.Middleware.Mapper
{
	public static class ErrorMapper
	{
		public static ErrorDetails MapErrorFrom(Exception exception, HttpContext context)
		{
			Contract.Assume(context != null && exception != null);
			string request = MapRequestPath(context.Request);
			HttpStatusCode status = MapExceptionStatusCode(exception);

			return new ErrorDetails()
			{
				StatusCode = (int)status,
				Title = "Internal Server Error",
				Error = exception.Message,
				Source = exception.Source,
				RootCause = exception.InnerException?.Message,
				Request = request
			};
		}

		public static HttpStatusCode MapExceptionStatusCode(Exception exception)
		{
			if (exception is UriFormatException)
				return HttpStatusCode.BadRequest;
			if (exception is TimeoutException)
				return HttpStatusCode.GatewayTimeout;
			return HttpStatusCode.InternalServerError;
		}

		public static string MapRequestPath(HttpRequest request)
		{
			Contract.Assume(request != null);

			return $"{request.Method}: {request.Path}{request.QueryString}";
		}

		public static string ToJsonString(this ErrorDetails error)
		{
			return JsonSerializer.Serialize(error);
		}
	}
}