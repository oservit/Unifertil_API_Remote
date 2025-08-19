namespace Application.Services.Products
{
    public interface IProductRemoteAppService
    {
        Task<List<ProductRemoteDto>?> ListAllAsync();
        Task<ProductRemoteDto?> GetByIdAsync(long id);
    }
}
