namespace WebProject.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception on {Path}", context.Request.Path);
            logger.LogError(ex.Message, context.User.Identity?.Name);

            context.Response.Redirect($"/Home/Error?errorMessage={Uri.EscapeDataString(ex.Message)}");
        }
    }
}



//public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
//{
//    public async Task InvokeAsync(HttpContext context)
//    {
//        try
//        {
//            await next(context);
//        }
//        catch (Exception ex)
//        {
//            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
//            await HandleExceptionAsync(context, ex);
//        }
//    }

//    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
//    {
//        context.Response.ContentType = "application/json";

//        var (statusCode, message) = ex switch
//        {
//            KeyNotFoundException => (HttpStatusCode.NotFound, "Resource not found."),
//            UnauthorizedAccessException => (HttpStatusCode.Forbidden, "Access denied."),
//            ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
//            InvalidOperationException => (HttpStatusCode.BadRequest, ex.Message),
//            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
//        };

//        context.Response.StatusCode = (int)statusCode;

//        var body = JsonSerializer.Serialize(new
//        {
//            error = message,
//            status = (int)statusCode,
//            traceId = context.TraceIdentifier
//        });

//        return context.Response.WriteAsync(body);
//    }
//}