using UnityEngine;

namespace Common
{
    public abstract class AManager<T> : ASingleton<T> where T : Component
    {
        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            if (IsInitialized)
            {
                Utilities.LogError($"{typeof(T).Name} is already initialized!");
                return;
            }
            
            IsInitialized = true;
            Utilities.Log($"{typeof(T).Name} is initialized!");
            AddListeners();
            OnInitialize();
        }

        protected virtual void OnInitialize() {}

        public void Dispose()
        {
            if (!IsInitialized)
            {
                Utilities.LogError($"{typeof(T).Name} is already disposed!");
                return;
            }
            
            IsInitialized = false;
            Utilities.Log($"{typeof(T).Name} is disposed!");
            RemoveListeners();
            OnDispose();
        }

        protected virtual void OnDispose() {}

        protected virtual void AddListeners() {}
        
        protected virtual void RemoveListeners() {} 
    }
}