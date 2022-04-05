using System;
using System.Threading.Tasks;

namespace JvLib.Services
{
    public class ServiceReference<T> where T : class
    {
        public delegate void OnReferenceStateDelegate(T pInstance);

        private T _instance;

        /// <summary>
        /// Gets the cached reference to the service. Caches it first if it wasn't cached yet. Do not
        /// use in OnDestroy or OnDisable.
        /// </summary>
        public T Reference => _instance ??= ServiceLocator.Instance.GetInstance<T>();

        public bool Exists => ServiceLocator.Instance.HasService<T>();

        public bool IsReady => ServiceLocator.Instance.IsServiceReady<T>();

        /// <summary>
        /// Wait for this reference to be registered and call the callback when ready. If it already
        /// exists, it will be called immediately.
        /// </summary>
        public void WaitForInstanceRegistered(OnReferenceStateDelegate pCallback)
        {
            if (Exists)
            {
                pCallback(Reference);
            }
            else
            {
                ServiceLocator.Instance.WaitForInstanceRegister<T>(
                    (serviceInstance) => pCallback((T) serviceInstance));
            }
        }

        /// <summary>
        /// Wait for this reference to be registered and call the callback when ready. If it already
        /// exists, it will be called immediately.
        /// </summary>
        public void WaitForInstanceRegistered(Action callback)
        {
            WaitForInstanceRegistered((pInstance) => callback());
        }

        public async Task<T> WaitForInstanceRegisteredAsync()
        {
            while (!Exists)
                await Task.Yield();

            return Reference;
        }

        /// <summary>
        /// Wait for this reference to be registered and call the callback when ready. If it already
        /// exists, it will be called immediately.
        /// </summary>
        public void WaitForInstanceReady(OnReferenceStateDelegate callback)
        {
            if (IsReady)
            {
                callback(Reference);
            }
            else
            {
                ServiceLocator.Instance.WaitForInstanceReady<T>(
                    (serviceInstance) => callback((T) serviceInstance));
            }
        }

        /// <summary>
        /// Wait for this reference to be registered and call the callback when ready. If it already
        /// exists, it will be called immediately.
        /// </summary>
        public void WaitForInstanceReady(Action callback)
        {
            WaitForInstanceReady((pInstance) => callback());
        }

        public async Task<T> WaitForInstanceReadyAsync()
        {
            while (!IsReady)
                await Task.Yield();

            return Reference;
        }

        /// <summary>
        /// Gets the cached reference to the service. Use in OnDestroy or OnDisable.
        /// </summary>
        public T CachedReference => _instance;

        public static implicit operator T(
            ServiceReference<T> serviceReference)
        {
            return serviceReference.Reference;
        }

        public void ClearCache()
        {
            _instance = null;
        }
    }
}
