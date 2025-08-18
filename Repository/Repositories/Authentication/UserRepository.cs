using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Domain.Features.Authentication;
using Infrastructure.Repositories.Authentication;

namespace Infrastructure.Repositories.Authentication
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDataContext context) : base(context)
        {
        }
    }
}
