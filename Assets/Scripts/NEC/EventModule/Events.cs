using System;
using System.Collections.Generic;

namespace NEC.EventModule
{
    public static class Events
    {
        private static readonly Dictionary<Type, IEvent> _events = new();

        public static T Get<T>() where T : IEvent, new()
        {
            var eventType = typeof(T);
            if (_events.TryGetValue(eventType, out var iEvent))
            {
                return (T)iEvent;
            }

            iEvent = (IEvent)Activator.CreateInstance(eventType);
            _events.Add(eventType, iEvent);
            return (T)iEvent;
        }

        public static void Dispose()
        {
            foreach (var pair in _events)
            {
                pair.Value.Dispose();
            }

            _events.Clear();
        }
    }
}
