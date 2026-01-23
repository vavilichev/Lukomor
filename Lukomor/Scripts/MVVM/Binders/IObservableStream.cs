using System;

namespace Lukomor.MVVM.Binders
{
    public interface IObservableStream<out T>
    {
        IObservable<T> OutputStream { get; }
    }
}