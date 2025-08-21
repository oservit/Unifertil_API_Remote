using Application.Common;
using Application.Common.Sync;
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
        private readonly IConfiguration _config;

        protected SyncRemoteServiceBase(
            IApiClient apiClient,
            ITokenService tokenService,
            IConfiguration config)
            : base(apiClient, tokenService, config)
        {
            _config = config;
        }

        protected abstract string GetRoute();

        /// <summary>
        /// Envia a mensagem para a remote correspondente ao ReceiverId
        /// </summary>
        public virtual async Task<DataResult> SyncRemote(SyncMessage<TModel> message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var remote = _config.GetSection("Remotes")
                                .GetChildren()
                                .Select(r => new
                                {
                                    Id = int.Parse(r["Id"]!),
                                    Url = r["Url"]?.TrimEnd('/'),
                                    User = r["User"],
                                    Password = r["Password"] 
                                })
                                .FirstOrDefault(r => r.Id == message.ReceiverId);

            if (remote == null || string.IsNullOrEmpty(remote.Url))
                throw new InvalidOperationException($"Remote com Id {message.ReceiverId} não encontrado ou sem URL");

            var url = $"{remote.Url}/{GetRoute()}/Sync/Receive";

            var credentiais = new RemoteCredentials(remote.User, remote.Password , remote.Url);

            return await PostAsync<DataResult>(url, message, credentiais);
        }
    }
}
