using System;
using System.Collections.Generic;

namespace KOTF.Core.Services
{
    public class ServiceProvider
    {
        private static ServiceProvider _serviceProvider;
        private readonly Dictionary<Type, IService> _serviceTypesToInstances = new();

        public static ServiceProvider GetInstance()
        {
            _serviceProvider ??= new ServiceProvider();
            return _serviceProvider;
        }

        public void RegisterService<TService>()
            where TService : class, IService
        {
            if (_serviceTypesToInstances.ContainsKey(typeof(TService)))
                throw new ArgumentException($"{typeof(TService)} has already been registered!");

            _serviceTypesToInstances.Add(typeof(TService), Activator.CreateInstance(typeof(TService)) as TService);
        }

        public TService Get<TService>()
            where TService : class, IService
        {
            Type serviceType = typeof(TService);
            if (!_serviceTypesToInstances.ContainsKey(serviceType))
                throw new ArgumentException($"{serviceType} has never been registered!");

            return _serviceTypesToInstances[serviceType] as TService;
        }
    }
}
