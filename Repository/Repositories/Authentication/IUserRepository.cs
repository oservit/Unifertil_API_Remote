using Domain.Features.Authentication;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories.Authentication
{
    public interface IUserRepository : IRepositoryBase<User>
    {
    }
}
