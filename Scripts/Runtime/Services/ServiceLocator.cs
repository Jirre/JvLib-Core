using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace JvLib.Services
{
    public class ServiceLocator
    {
        private static ServiceLocator _instance;

        public static ServiceLocator Instance =>
            _instance ??= new ServiceLocator();

        internal delegate void OnInstanceStateDelegate(object pInstance);

        private readonly Dictionary<Type, IService> _register;

        private ServiceLocator()
        {
            _register = new Dictionary<Type, IService>();
            _onRegisterCallbacks = new Dictionary<Type, OnInstanceStateDelegate>();
            _onReadyCallbacks = new Dictionary<Type, OnInstanceStateDelegate>();
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            _instance = null;
        }

        #region --- GET ---

        internal T GetInstance<T>() where T : class
        {
            return GetInstance(typeof(T)) as T;
        }

        private object GetInstance(Type pType)
        {
            if (_register.ContainsKey(pType))
            {
                return _register[pType];
            }

            return null;
        }

        #endregion

        #region --- ON REGISTER ---

        public void Register<T>(T pInstance) where T : IService
        {
            Type type = pInstance.GetType();
            if (HasService(type))
            {
                Debug.LogError($"Service of Type {type.FullName} already exists in the register.");
                return;
            }

            _register.Add(type, pInstance);
            DispatchOnRegisteredEvent(type);
        }

        internal bool HasService<T>() where T : class
        {
            return HasService(typeof(T));
        }

        private bool HasService(Type pType)
        {
            return _register.ContainsKey(pType);
        }

        private readonly Dictionary<Type, OnInstanceStateDelegate> _onRegisterCallbacks;

        private void DispatchOnRegisteredEvent(Type pType)
        {
            if (!_onRegisterCallbacks.ContainsKey(pType)) return;

            OnInstanceStateDelegate delegates = _onRegisterCallbacks[pType];
            if (delegates == null)
                return;

            delegates(GetInstance(pType));
            delegates = null;
            _onRegisterCallbacks[pType] = null;
        }

        internal void WaitForInstanceRegister<T>(OnInstanceStateDelegate pCallback)
        {
            WaitForInstanceRegister(typeof(T), pCallback);
        }

        private void WaitForInstanceRegister(Type pType, OnInstanceStateDelegate pCallback)
        {
            if (HasService(pType))
            {
                pCallback(GetInstance(pType));
            }
            else
            {
                if (_onRegisterCallbacks.ContainsKey(pType))
                {
                    _onRegisterCallbacks[pType] += pCallback;
                }
                else
                {
                    _onRegisterCallbacks[pType] = pCallback;
                }
            }
        }

        #endregion

        #region --- ON READY ---

        public void ReportInstanceReady<T>(T pInstance) where T : IService
        {
            Type type = pInstance.GetType();
            if (!HasService(type))
            {
                Debug.LogError($"Service of Type {type.FullName} does not exists in the register.");
                return;
            }

            DispatchOnReadyEvent(type);
        }

        internal bool IsServiceReady<T>() where T : class
        {
            return IsServiceReady(typeof(T));
        }

        private bool IsServiceReady(Type pType)
        {
            return HasService(pType) && _register[pType].IsServiceReady;
        }

        private readonly Dictionary<Type, OnInstanceStateDelegate> _onReadyCallbacks;

        private void DispatchOnReadyEvent(Type pType)
        {
            if (!_onReadyCallbacks.ContainsKey(pType)) return;
            OnInstanceStateDelegate delegates = _onReadyCallbacks[pType];
            if (delegates == null)
                return;

            delegates(GetInstance(pType));
            delegates = null;
            _onReadyCallbacks[pType] = null;
        }

        internal void WaitForInstanceReady<T>(OnInstanceStateDelegate pCallback)
        {
            WaitForInstanceReady(typeof(T), pCallback);
        }

        private void WaitForInstanceReady(Type pType, OnInstanceStateDelegate pCallback)
        {
            if (IsServiceReady(pType))
            {
                pCallback(GetInstance(pType));
            }
            else
            {
                if (_onReadyCallbacks.ContainsKey(pType))
                {
                    _onReadyCallbacks[pType] += pCallback;
                }
                else
                {
                    _onReadyCallbacks[pType] = pCallback;
                }
            }
        }

        #endregion

        #region --- REMOVE ---

        public void Remove<T>(T pInstance)
        {
            if (!HasService(pInstance.GetType()))
                return;
            if (pInstance is IDisposable)
                ((IDisposable)pInstance).Dispose();
            
            _register.Remove(pInstance.GetType());
        }

        public void RemoveAll()
        {
            foreach (KeyValuePair<Type, IService> kvPair in _register)
            {
                if (kvPair.Value is IDisposable)
                    (kvPair.Value as IDisposable)?.Dispose();
            }
            _register.Clear();
        }

        #endregion
    }
}
