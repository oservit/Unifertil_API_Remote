using System;

namespace Service.Base
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public T GetService<T>()
        {
            var service = _serviceProvider.GetService(typeof(T));
            if (service == null)
                throw new InvalidOperationException($"Service of type {typeof(T)} could not be resolved.");

            return (T)service;
        }
    }
}
