using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An error occured : {Message}", exception.Message);

            var statusCode = GetExceptionStatusCode(exception);
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = GetExceptionTitle(exception),
                Detail = GetExceptionDetails(exception),
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

            return true;
        }

        private static string GetExceptionTitle(Exception exception)
        {
            return exception switch
            {
                BusinessRuleException => "Business rule violation",
                ProductNotFoundException => "Product not found",
                _ => "An unexpected error occurred"
            };
        }
        private static string GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                BusinessRuleException => exception.Message,
                ProductNotFoundException => exception.Message,
                _ => "An unexpected error occurred"
            };
        }
        private static int GetExceptionStatusCode(Exception exception)
        {
            return exception switch
            {
                ProductNotFoundException => StatusCodes.Status404NotFound,
                BusinessRuleException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
        }
    }
}
