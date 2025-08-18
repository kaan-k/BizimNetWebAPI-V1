using Core.Utilities.Context;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Core.Infrastructure
{
    public class HttpUserContext : IUserContext
    {
        private readonly IHttpContextAccessor _http;

        public HttpUserContext(IHttpContextAccessor http) => _http = http;

        public string? UserId
        {
            get
            {
                var ctx = _http.HttpContext;
                if (ctx is null) return null;
                var fromHeader = ctx.Request.Headers["X-User-Id"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(fromHeader)) return fromHeader;
                var claim = ctx.User?.Claims?.FirstOrDefault(c =>
                    c.Type == ClaimTypes.NameIdentifier ||
                    c.Type == "sub" ||
                    c.Type.Contains("nameidentifier"));

                return claim?.Value;
            }
        }

        public bool IsAuthenticated =>
            _http.HttpContext?.User?.Identity?.IsAuthenticated == true;
    }
}
