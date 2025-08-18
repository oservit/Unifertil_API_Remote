using Domain.Features.Authentication;
using Infrastructure.Repositories.Authentication;

namespace Service.Features.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }
        public async Task<bool> ValidateUserCredentials(User user)
        {
            try
            {
                var existingUser = await _userRepository.Get(x => x.Username == user.Username);

                if (existingUser == null || existingUser.Password != user.Password || existingUser.IsActive == false)
                    return false;

                user.Id = existingUser.Id;

                return true;
            }
            catch
            {
                throw;
            }
        }

    }
}
