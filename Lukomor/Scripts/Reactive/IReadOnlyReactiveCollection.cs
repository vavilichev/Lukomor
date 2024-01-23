using System;
using System.Collections.Generic;

namespace Lukomor.Reactive
{
    public interface IReadOnlyReactiveCollection<out T> : IReadOnlyCollection<T>
    {
        IObservable<T> Added { get; }
        IObservable<T> Removed { get; }
    }
}