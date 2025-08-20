using Domain.Common;

namespace Infrastructure.Repositories.Common
{
    public class UpdateObjectParameters
    {
        public long Id { get; set; }
        public dynamic UpdateFields { get; set; }
        public HttpUser User { get; set; }
    }
}
