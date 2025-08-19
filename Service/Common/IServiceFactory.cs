namespace Service.Common
{
    public interface IServiceFactory
    {
        T GetService<T>();
    }
}
