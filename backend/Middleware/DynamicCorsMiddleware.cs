using Microsoft.EntityFrameworkCore;

public class DynamicCorsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;

    public DynamicCorsMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var origin = context.Request.Headers["Origin"].FirstOrDefault();

        if (!string.IsNullOrEmpty(origin))
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var allowedOrigin = await dbContext.AllowedOrigins
                .FirstOrDefaultAsync(o => o.Origin == origin);

            if (allowedOrigin == null)
            {
                // Insert as pending
                dbContext.AllowedOrigins.Add(new AllowedOrigin { Origin = origin, Status = "Pending" });
                await dbContext.SaveChangesAsync();
            }
            else if (allowedOrigin.Status == "Rejected")
            {
                // Reject access
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access Denied: Origin is Rejected.");
                return;
            }
            else if (allowedOrigin.Status == "Approved")
            {
                // Allow only if approved
                context.Response.Headers.Add("Access-Control-Allow-Origin", origin);
                context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
                context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");

                if (context.Request.Method == "OPTIONS")
                {
                    context.Response.StatusCode = 200;
                    return;
                }
            }
        }

        await _next(context);
    }
}
