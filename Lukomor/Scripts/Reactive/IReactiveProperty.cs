using System;

namespace Lukomor.Reactive
{
    public interface IReactiveProperty<out T> : IObservable<T>
    {
        T Value { get; }
        bool HasValue { get; }

        void Unsubscribe(IObserver<T> observer);
    }
}