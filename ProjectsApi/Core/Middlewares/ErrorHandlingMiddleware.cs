
using Newtonsoft.Json;
using projects_api.Core.Models;
using System.Net;

namespace projects_api.Core.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (InnerException ie)
            {
                await Send(context, ie);
            }
            catch (Exception ex)
            {
                var ie = new InnerException((int)HttpStatusCode.InternalServerError,ex.Message);
                _logger.LogError(ex, ex.Message);
                await Send(context, ie);
            }
        }

        public async Task Send(HttpContext context, InnerException innerException)
        {
            var response = new ApiResult<string>
            {
                Success = false,
                StatusCode = innerException.ErrorCode,
                Message = innerException.Message
            };

            context.Response.StatusCode = innerException.ErrorCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
