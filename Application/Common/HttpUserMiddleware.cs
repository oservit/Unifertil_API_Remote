using Microsoft.AspNetCore.Http;

namespace Application.Common
{
    public class HttpUserMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}
