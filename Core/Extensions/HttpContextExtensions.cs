using Microsoft.AspNetCore.Http;

public static class HttpContextExtensions
{
    public static string? GetUserId(this HttpContext ctx)
        => ctx.Request.Headers["X-User-Id"].FirstOrDefault();
}
