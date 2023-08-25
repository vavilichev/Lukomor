using System;
using System.Collections.Generic;

namespace Lukomor.Reactive
{
    public interface IReactiveCollection<T> : IEnumerable<T>
    {
        int Count { get; }

        void AddListenerItemAdded(Action<T> callback);
        void RemoveListenerItemAdded(Action<T> callback);
        void AddListenerItemRemoved(Action<T> callback);
        void RemoveListenerItemRemoved(Action<T> callback);
    }
}