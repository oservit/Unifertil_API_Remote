using Domain.Base;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Http
{
    public class HttpUserAccessor : IHttpUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpUser User
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;

                if (context?.User?.Identity?.IsAuthenticated != true)
                    return new HttpUser();

                var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var username = context.User.Identity.Name;

                return new HttpUser
                {
                    Id = int.TryParse(idClaim, out var id) ? id : 0,
                    Username = username
                };
            }
        }
    }
}
