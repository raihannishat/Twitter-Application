using System.Security.Claims;

namespace WebApi.Middleware;

public class LoggerMiddleware
{
    private readonly RequestDelegate next;

    public LoggerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public Task Invoke(HttpContext httpContext, IDiagnosticContext diagnosticContext)
    {
        diagnosticContext.Set("UserId", httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        diagnosticContext.Set("UserEmail", httpContext.User.FindFirstValue(ClaimTypes.Email));

        return next(httpContext);
    }
}
