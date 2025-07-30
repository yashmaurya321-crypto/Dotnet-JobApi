namespace backend.Middleware
{
    public class AdminMiddleware
    {
        private RequestDelegate _next;
        public AdminMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/admin")) { 
                if (!context.Request.Headers.TryGetValue("X-Is-Admin", out var isAdmin) || isAdmin != "true")
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Access denied: Admins only.");
                    return;
                }
            }
             await _next(context);
        }
    }
}
