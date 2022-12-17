using System;
using System.Collections.Generic;
using System.Linq;

namespace KOTF.Core.Services
{
    public class ServiceProvider
    {
        private static ServiceProvider _serviceProvider;
        private readonly HashSet<Type> _serviceTypes = new();
        private readonly List<IService> _services = new();

        public static ServiceProvider GetInstance()
        {
            if (_serviceProvider == null)
                _serviceProvider = new ServiceProvider();

            return _serviceProvider;
        }

        public void RegisterService<TService>()
            where TService : class, IService
        {
            if (_serviceTypes.Contains(typeof(TService)))
                throw new ArgumentException($"{typeof(TService)} has already been registered!");

            _serviceTypes.Add(typeof(TService));
            _services.Add(Activator.CreateInstance(typeof(TService)) as TService);
        }

        public TService Get<TService>()
            where TService : class, IService
        {
            if (!_serviceTypes.Contains(typeof(TService)))
                throw new ArgumentException($"{typeof(TService)} has never been registered!");

            return _services.FirstOrDefault(x => x.GetType() == typeof(TService)) as TService;
        }
    }
}
