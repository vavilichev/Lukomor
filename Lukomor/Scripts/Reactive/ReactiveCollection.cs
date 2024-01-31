using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Lukomor.Reactive
{
    public sealed class ReactiveCollection<T> : IReadOnlyReactiveCollection<T>, ICollection<T>
    {
        public int Count => _items.Count;
        public bool IsReadOnly => false;
        public IObservable<T> Added { get; }
        public IObservable<T> Removed { get; }
        
        private Action<T> _itemAdded;
        private Action<T> _itemRemoved;
        
        private readonly List<T> _items = new();
        
        public ReactiveCollection()
        {
            Added = _items.ToObservable(Scheduler.Immediate)
                .Concat(Observable.FromEvent<T>(
                    a => _itemAdded += a,
                    a => _itemAdded -= a, Scheduler.Immediate));

            Removed = Observable.FromEvent<T>(
                a => _itemRemoved += a,
                a => _itemRemoved -= a, Scheduler.Immediate);
        }

        public ReactiveCollection(IEnumerable<T> collection) : this()
        {
            _items.AddRange(collection);
        }
        
        public void Add(T item)
        {
            _items.Add(item);
            
            _itemAdded?.Invoke(item);
        }
        
        public bool Remove(T item)
        {
            var wasRemoved = _items.Remove(item);
            
            if (wasRemoved)
            {
                _itemRemoved?.Invoke(item);
            }
            
            return wasRemoved;
        }
        
        public void Clear()
        {
            foreach (var item in _items)
            {
                _itemRemoved?.Invoke(item);
            }
            
            _items.Clear();
        }
        
        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}