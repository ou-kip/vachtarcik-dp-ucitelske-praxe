using Core.Domain.Responses;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace NotificationService.Middleware
{
    /// <summary>
    /// The middleware for exception handling
    /// </summary>
    public class ExceptionMiddleware
    {
        /// <summary>
        /// the request
        /// </summary>
        private readonly RequestDelegate _request;

        /// <summary>
        /// .ctor for ExceptionMiddleware
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ExceptionMiddleware(RequestDelegate request)
        {
            _request = request;
        }

        /// <summary>
        /// Invokes the handling
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (Exception ex)
            {
                await Handle(context, ex);
            }
        }

        /// <summary>
        /// Handles the exceptions
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private Task Handle(HttpContext context, Exception ex)
        {
            var code = ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                NotImplementedException => StatusCodes.Status501NotImplemented,
                ValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;

            return context.Response.WriteAsync(
                JsonSerializer.Serialize(
                    new BaseResponse()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = ex.Message
                    }).ToString());
        }
    }
}
