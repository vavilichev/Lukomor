using System;
using System.Collections;
using System.Collections.Generic;

namespace Lukomor.Reactive
{
    public class ReactiveCollection<T> : IReactiveCollection<T>
    {
        public int Count => _items.Count;

        private readonly List<T> _items = new();
        private readonly List<Action<T>> _addedCallbacks = new();
        private readonly List<Action<T>> _removedCallbacks = new();

        public void Add(T item)
        {
            _items.Add(item);
            NotifyAdded(item);
        }

        public void Remove(T item)
        {
            if (_items.Remove(item))
            {
                NotifyRemoved(item);
            }
        }

        public void AddListenerItemAdded(Action<T> callback)
        {
            _addedCallbacks.Add(callback);
        }

        public void RemoveListenerItemAdded(Action<T> callback)
        {
            _addedCallbacks.Remove(callback);
        }

        public void AddListenerItemRemoved(Action<T> callback)
        {
            _removedCallbacks.Add(callback);
        }

        public void RemoveListenerItemRemoved(Action<T> callback)
        {
            _removedCallbacks.Remove(callback);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void NotifyAdded(T item)
        {
            foreach (var callback in _addedCallbacks)
            {
                callback?.Invoke(item);
            }
        }

        private void NotifyRemoved(T item)
        {
            foreach (var callback in _removedCallbacks)
            {
                callback?.Invoke(item);
            }
        }
    }
}