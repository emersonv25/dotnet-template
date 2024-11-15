using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Template.Api.DTOs;

namespace Template.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error occurred.");
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, "Validation failed", new[] { ex.Message });
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "A required argument was null.");
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, "Required argument is missing", new[] { ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Resource not found.");
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, "Resource not found", new[] { ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt.");
                await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, "Unauthorized access", new[] { ex.Message });
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "Operation timed out.");
                await HandleExceptionAsync(context, HttpStatusCode.GatewayTimeout, "The operation timed out", new[] { ex.Message });
            }
            catch (Exception ex)
            {
                // Para outras exceções, utilize um tratamento genérico
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred", new[] { ex.Message });
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message, IEnumerable<string> errors)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new ErrorResponseDto(context.Response.StatusCode, message, errors);

            return context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
    }

}
