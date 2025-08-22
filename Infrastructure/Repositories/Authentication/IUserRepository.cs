using Domain.Features.Authentication;
using Infrastructure.Repositories.Common;

namespace Infrastructure.Repositories.Authentication
{
    public interface IUserRepository : IRepositoryBase<User>
    {
    }
}
