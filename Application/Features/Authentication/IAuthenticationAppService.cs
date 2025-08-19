using Libs.Common;

namespace Application.Features.Authentication
{
    public interface IAuthenticationAppService
    {
        Task<DataResult> GetToken(UserViewModel userModel);
    }
}
