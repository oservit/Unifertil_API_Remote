namespace Application.Services.Clients
{
    public interface IClientRemoteAppService
    {
        Task<List<ClientRemoteDto>?> ListAllAsync();
        Task<ClientRemoteDto?> GetByIdAsync(long id);
    }
}
