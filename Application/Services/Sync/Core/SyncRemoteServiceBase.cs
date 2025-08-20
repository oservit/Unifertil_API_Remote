using Application.Common;
using Application.Services.Auth;
using Application.Services.Core;
using Infrastructure.Http;
using Libs.Common;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Sync.Core
{
    public abstract class SyncRemoteServiceBase<TModel> : AuthenticatedAppService, ISyncRemoteServiceBase<TModel>
        where TModel : class, IViewModelBase
    {
        private readonly string _baseUrl;

        protected SyncRemoteServiceBase(
            IApiClient apiClient,
            ITokenService tokenService,
            IConfiguration config,
            string route)
            : base(apiClient, tokenService)
        {
            _baseUrl = $"{config["Central:Url"].TrimEnd('/')}/{route}/Sync";
        }

        public virtual async Task<DataResult> SyncRemote(TModel model)
        {
            return await PostAsync<DataResult>($"{_baseUrl}/Receive", model);
        }
    }
}
