using Infrastructure.Data;
using Infrastructure.Repositories.Common;
using Domain.Features.Authentication;

namespace Infrastructure.Repositories.Authentication
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDataContext context) : base(context)
        {
        }
    }
}
