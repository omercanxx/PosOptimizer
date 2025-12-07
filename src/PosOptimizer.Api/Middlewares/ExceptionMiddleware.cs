using FluentValidation;
using PosOptimizer.Common;
using PosOptimizer.Common.Enums;
using PosOptimizer.Common.Extensions;

namespace PosOptimizer.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            var firstError = ex.Errors.FirstOrDefault()?.ErrorMessage 
                             ?? ErrorCode.UnknownError.GetDescription();

            _logger.LogWarning("Validation error: {Message}", firstError);

            var result = ApiResult<object>.Fail(
                ErrorCode.ValidationError,
                firstError
            );

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred.");

            var result = ApiResult<object>.Fail(
                ErrorCode.UnknownError
            );

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}