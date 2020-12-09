using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace LSInjector
{
    /// <summary>
    /// A base class for LSInjector services.
    /// </summary>
    public abstract class LSService
    {
        // Just a convenient wrapper for getting services from LSInjector
        [CanBeNull]
        protected T SafeGetService<T>() where T : LSService =>
            LSInjector.Instance.GetService<T>();

        // Just a convenient wrapper for getting services from LSInjector
        protected T GetService<T>() where T : LSService
        {
            var service = SafeGetService<T>();
            if (service != null) return service;

            var msg = $"Service {typeof(T).Name} was requested by service {GetType().Name}, " +
                      "but it is not currently loaded.";
            Debug.LogError(msg);
            throw new InvalidOperationException(msg);
        }

        // You have to pass "this" to enable type detection
        protected void RemoveThisService<T>(T thisInstance) where T : LSService => 
            LSInjector.Instance.RemoveService(thisInstance);

        // Add your disposables here, so they will be destroyed automatically on service destroy.
        // Useful to deal with UniRx observers disposal.
        public List<IDisposable> Disposables { get; } = new List<IDisposable>();

        protected void AddDisposable(IDisposable disposable)
        {
            Disposables.Add(disposable);
        }


        public virtual void PreInit()
        {
        }

        public virtual void Init()
        {
        }

        public virtual void PostInit()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void PreDestroy()
        {
        }

        public virtual void Destroy()
        {
        }
    }
}