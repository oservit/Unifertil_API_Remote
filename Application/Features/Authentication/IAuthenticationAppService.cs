using Libs.Base;

namespace Application.Features.Authentication
{
    public interface IAuthenticationAppService
    {
        Task<DataResult> GetToken(UserViewModel userModel);
    }
}
