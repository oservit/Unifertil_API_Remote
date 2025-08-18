using Domain.Base;

namespace Infrastructure.Http
{
    public interface IHttpUserAccessor
    {
        HttpUser User { get; }
    }
}
