using Domain.Features.Authentication;

namespace Service.Features.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> ValidateUserCredentials(User user);
    }
}
