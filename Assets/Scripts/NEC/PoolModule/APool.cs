using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace NEC.PoolModule
{
    public abstract class APool<T> : IPool where T : Component, IPooled
    {
        private ObjectPool<T> _pool;
        private bool _initialized;
        private T _prefab;
        private Transform _root;

        public void Initialize(T prefab, Transform root, int prefill = 0,
            int capacity = 10, int maxSize = 10000, bool collectionCheck = true)
        {
            if (_initialized)
                throw new InvalidOperationException($"{GetType()} is already initialized.");

            _initialized = true;
            _prefab = prefab;
            _root = root;
            _pool = new ObjectPool<T>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy,
                collectionCheck, capacity, maxSize);

            Fill(prefill);
        }

        public T Get()
        {
            if (!_initialized)
                throw new InvalidOperationException($"{GetType()} is not initialized.");

            var obj = _pool.Get();
            obj.OnGet();
            return obj;
        }

        public void Release(T obj)
        {
            if (!_initialized)
                throw new InvalidOperationException($"{GetType()} is not initialized.");

            obj.OnRelease();
            _pool.Release(obj);
        }

        public void Fill(int amount)
        {
            if (!_initialized)
                throw new InvalidOperationException($"{GetType()} is not initialized.");

            if (amount <= 0)
                return;

            var objs = new List<T>();
            for (var i = 0; i < amount; i++)
                objs.Add(_pool.Get());
            foreach (var obj in objs)
                _pool.Release(obj);
        }

        public void Dispose()
        {
            if (!_initialized)
                return;

            _pool.Dispose();
            _pool = null;
            _initialized = false;
        }

        private T CreateFunc()
        {
            var obj = UnityEngine.Object.Instantiate(_prefab, _root);
            obj.gameObject.SetActive(false);
            return obj;
        }

        private void ActionOnGet(T obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void ActionOnRelease(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_root);
        }

        private void ActionOnDestroy(T obj)
        {
            UnityEngine.Object.Destroy(obj.gameObject);
        }
    }
}
