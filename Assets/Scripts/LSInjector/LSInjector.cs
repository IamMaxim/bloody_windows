using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Debug = UnityEngine.Debug;


namespace LS.LSInjector
{
    public class LSInjector : MonoBehaviour
    {
        private Dictionary<Type, LSService> services = new Dictionary<Type, LSService>();
        private static LSInjector _instance;

        // Indicates if services has changed during the last frame, and in this case, on next update the appropriate
        // events will be fired on corresponding services.
        // Only has effect after the initialization (when update cycle has started).
        // Used to improve performance (simply check a boolean on each update instead of obtaining list lengths and
        // comparing them).
        private bool isDirty = false;
        private List<LSService> servicesToAdd = new List<LSService>();
        private List<Type> servicesToRemove = new List<Type>();

        // Becomes true when the Awake and Start methods are finished.
        private bool initializationCompleted = false;

        public LSInjectorConfig Config;


        public static LSInjector Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException(
                        "LSInjector is not yet initialized, but you are already trying to access it");
                return _instance;
            }
            set
            {
                if (_instance != null)
                    _instance.Destroy();

                _instance = value;
            }
        }

        private void Awake()
        {
            _instance = this;

            if (Config == null)
                Debug.LogWarning("LSInjectorConfig is not specified. If you want to perform initialization," +
                                 " create LSInjectorConfig and attach it to LSInjector in the inspector.");
            else
            {
                Debug.Log("Adding services from config");
                Config.Setup(this);
            }

            Debug.Log("Going to PreInit services");
            PreInit();
            Debug.Log("PreInit done");
        }

        
        private void Start()
        {
            Debug.Log("Going to Init services");
            Init();
            Debug.Log("Init done");
            PostInit();
            Debug.Log("PostInit done");
            initializationCompleted = true;
        }

        private void OnApplicationQuit()
        {
            Destroy();
        }


        // Called on LSInjector creation (Unity Awake event). 
        public void PreInit() =>
            services.Values.ToList().ForEach(v => v.PreInit());

        // Propagate Start Unity event to all services.
        public void Init() => services.Values.ToList().ForEach(v => v.Init());

        public void PostInit() => services.Values.ToList().ForEach(v => v.PostInit());


        // Propagate Update Unity event to all services.
        public void Update()
        {
            if (isDirty)
            {
                lock (services)
                {
                    lock (servicesToAdd)
                    {
                        servicesToAdd.ForEach(service => services.Add(service.GetType(), service));
                        servicesToAdd.ForEach(service => service.PreInit());
                        servicesToAdd.ForEach(service => service.Init());
                        servicesToAdd.ForEach(service => service.PostInit());
                        servicesToAdd.Clear();
                    }

                    lock (servicesToRemove)
                    {
                        // Check that all services that we are trying to remove exist
                        foreach (var type in servicesToRemove)
                        {
                            services.TryGetValue(type, out var service);
                            
                            if (service != null) continue;
                            
                            servicesToRemove.Remove(type);
                            throw new InvalidOperationException($"Service with type {type} is not added, " +
                                                                "but you are trying to remove it.");
                        }

                        servicesToRemove.ForEach(type => services[type].PreDestroy());
                        servicesToRemove.ForEach(type => services[type].Destroy());
                        servicesToRemove.ForEach(type => services.Remove(type));
                        servicesToRemove.Clear();
                    }

                    isDirty = false;
                }
            }

            services.Values.ToList().ForEach(v => v.Update());
        }

        // Called on LSInjector destroy. May be Unity OnApplicationQuit event, but this is not necessary.
        public void Destroy()
        {
            services.Values.ToList().ForEach(v => v.PreDestroy());
            services.Values.ToList().ForEach(v =>
            {
                // Clear all disposables before destroying (so there will definitely be no events from other threads
                // when the service is half-destroyed).
                v.Disposables.ForEach(d => d.Dispose());
                v.Disposables.Clear();

                v.Destroy();
            });
            services.Clear();
        }

        [CanBeNull]
        public T GetService<T>() where T : LSService
        {
            services.TryGetValue(typeof(T), out var service);
            return service as T;
        }

        public void AddService<T>(T service) where T : LSService
        {
            if (initializationCompleted)
                lock (servicesToAdd)
                {
                    servicesToAdd.Add(service);
                    isDirty = true;
                }
            else
                lock (services)
                {
                    // Destroy previous service of this type, if any, before adding new one
                    _RemoveService<T>();
                    services[typeof(T)] = service;
                }
        }

        public void AddContainer<T>(T container) where T : LSContainer
        {
            var newServices = container.CreateServices();
            newServices.ForEach(AddService);
        }

        public void RemoveService<T>(T service) where T : LSService
        {
            if (initializationCompleted)
                lock (servicesToRemove)
                {
                    servicesToRemove.Add(typeof(T));
                    isDirty = true;
                }
            else
                lock (services)
                {
                    _RemoveService<T>();
                }
        }

        private void _RemoveService<T>() where T : LSService
        {
            services.TryGetValue(typeof(T), out var service);
            service?.Destroy();
        }
    }
}