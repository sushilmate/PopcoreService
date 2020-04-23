
using Microsoft.AspNetCore.Builder;

namespace Popcore.API.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApiRateLimitMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiRateLimitMiddleware>();
        }
    }
}
