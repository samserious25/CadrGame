using System;
using System.Collections.Generic;

namespace Assets.Arch.Services
{
    public class GameServices
    {
        private static GameServices _instance;
        public static GameServices Container => _instance ??= new GameServices();
        private readonly Dictionary<Type, IService> _registeredServices = new();

        public TService Register<TService>(TService service) where TService : IService
        {
            if (!_registeredServices.ContainsKey(typeof(TService)))
            {
                _registeredServices.Add(typeof(TService), service);
                return service;
            }

            Logging.LogWarning("GameServices is already contains type " + service);
            return default;
        }

        public TService Get<TService>() where TService : IService
        {
            if (_registeredServices.ContainsKey(typeof(TService)))
                return (TService)_registeredServices[typeof(TService)];

            Logging.LogError("Service " + typeof(TService) + " is not registered in GameServices");
            return default;
        }

        public void UnRegister<TService>(TService service) where TService : IService
        {
            if (_registeredServices.ContainsKey(typeof(TService)))
                _registeredServices.Remove(typeof(TService));
            else
                Logging.LogError("Service " + typeof(TService) + " is not registered in GameServices");
        }

        public void UnRegisterAll() =>
            _registeredServices.Clear();
    }
}