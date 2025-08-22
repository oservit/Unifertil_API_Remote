using Domain.Common;

namespace Infrastructure.Http
{
    public interface IHttpUserAccessor
    {
        HttpUser User { get; }
    }
}
