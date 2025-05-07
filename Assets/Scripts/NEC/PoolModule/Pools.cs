using System;
using System.Collections.Generic;

namespace NEC.PoolModule
{
    public static class Pools
    {
        private static readonly Dictionary<Type, IPool> _pools = new();

        public static T Get<T>() where T : IPool, new()
        {
            var poolType = typeof(T);
            if (_pools.TryGetValue(poolType, out var pool))
            {
                return (T)pool;
            }

            pool = (IPool)Activator.CreateInstance(typeof(T));
            _pools.Add(poolType, pool);
            return (T)pool;
        }

        public static void Dispose()
        {
            foreach (var pair in _pools)
            {
                pair.Value.Dispose();
            }

            _pools.Clear();
        }
    }
}
