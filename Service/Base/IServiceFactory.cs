namespace Service.Base
{
    public interface IServiceFactory
    {
        T GetService<T>();
    }
}
