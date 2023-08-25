using System;

namespace Lukomor.Reactive
{
    public interface IReactiveProperty<T>
    {
        T Value { get; }

        void AddListener(Action<T> callback);
        void RemoveListener(Action<T> callback);
    }
}