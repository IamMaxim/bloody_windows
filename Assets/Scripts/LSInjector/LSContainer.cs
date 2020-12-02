using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace LS.LSInjector
{
    public abstract class LSContainer
    {
        // Just a convenient wrapper for getting services from LSInjector
        [CanBeNull]
        protected T SafeGetService<T>() where T : LSService =>
            LSInjector.Instance.GetService<T>();

        protected T GetService<T>() where T : LSService
        {
            var service = SafeGetService<T>();
            if (service != null) return service;

            var msg = $"Service {typeof(T).Name} was requested by container {GetType().Name}, " +
                      "but it is not currently loaded.";
            Debug.LogError(msg);
            throw new InvalidOperationException(msg);
        }

        public abstract List<LSService> CreateServices();
    }
}