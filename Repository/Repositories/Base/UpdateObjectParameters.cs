using Domain.Base;

namespace Infrastructure.Repositories.Base
{
    public class UpdateObjectParameters
    {
        public long Id { get; set; }
        public dynamic UpdateFields { get; set; }
        public HttpUser User { get; set; }
    }
}
