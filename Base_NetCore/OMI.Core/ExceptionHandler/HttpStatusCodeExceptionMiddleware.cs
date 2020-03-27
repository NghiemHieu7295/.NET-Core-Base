using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OMI.Core.ExceptionHandler
{
  public class HttpStatusCodeExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILog _logger;

    public HttpStatusCodeExceptionMiddleware(RequestDelegate next)
    {
      _next = next ?? throw new ArgumentNullException(nameof(next));
      _logger = _logger = LogManager.GetLogger(this.GetType());
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(context, ex);
      }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      var code = HttpStatusCode.InternalServerError;

      var result = string.Empty;

      switch (exception)
      {
        case ValidationException validationException:
          code = HttpStatusCode.BadRequest;
          result = JsonConvert.SerializeObject(validationException.Failures);
          break;
        case BadRequestException badRequestException:
          code = HttpStatusCode.BadRequest;
          result = badRequestException.Message;
          break;
        case NotFoundException _:
          code = HttpStatusCode.NotFound;
          break;
        case HttpStatusCodeException httpStatusCodeException:
          code = (HttpStatusCode)httpStatusCodeException.StatusCode;
          break;
        default:
          code = HttpStatusCode.InternalServerError;
          break;
      }

      context.Response.Clear();
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)code;

      if (result == string.Empty)
      {
#if DEBUG
        result = JsonConvert.SerializeObject(exception);
#else
        result = JsonConvert.SerializeObject(new { error = exception.Message });
#endif
      }

      return context.Response.WriteAsync(result);
    }
  }

  // Extension method used to add the middleware to the HTTP request pipeline.
  public static class HttpStatusCodeExceptionMiddlewareExtensions
  {
    public static IApplicationBuilder UseHttpStatusCodeExceptionMiddleware(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<HttpStatusCodeExceptionMiddleware>();
    }
  }
}
