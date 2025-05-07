using System;

namespace NEC.EventModule
{
    public abstract class AEvent : IEvent
    {
        private Action _callback;

        public void AddListener(Action handler)
        {
            _callback += handler;
        }

        public void RemoveListener(Action handler)
        {
            _callback -= handler;
        }

        public void Dispatch()
        {
            _callback?.Invoke();
        }

        public void Dispose()
        {
            _callback = null;
        }
    }

    public abstract class AEvent<T> : IEvent
    {
        private Action<T> _callback;

        public void AddListener(Action<T> handler)
        {
            _callback += handler;
        }

        public void RemoveListener(Action<T> handler)
        {
            _callback -= handler;
        }

        public void Dispatch(T arg1)
        {
            _callback?.Invoke(arg1);
        }

        public void Dispose()
        {
            _callback = null;
        }
    }

    public abstract class AEvent<T, U> : IEvent
    {
        private Action<T, U> _callback;

        public void AddListener(Action<T, U> handler)
        {
            _callback += handler;
        }

        public void RemoveListener(Action<T, U> handler)
        {
            _callback -= handler;
        }

        public void Dispatch(T arg1, U arg2)
        {
            _callback?.Invoke(arg1, arg2);
        }

        public void Dispose()
        {
            _callback = null;
        }
    }

    public abstract class AEvent<T, U, V> : IEvent
    {
        private Action<T, U, V> _callback;

        public void AddListener(Action<T, U, V> handler)
        {
            _callback += handler;
        }

        public void RemoveListener(Action<T, U, V> handler)
        {
            _callback -= handler;
        }

        public void Dispatch(T arg1, U arg2, V arg3)
        {
            _callback?.Invoke(arg1, arg2, arg3);
        }

        public void Dispose()
        {
            _callback = null;
        }
    }
}
