using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace Lukomor.MVVM
{
    public abstract class ViewModel : IViewModel, IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new();

        protected ICollection<IDisposable> Subscriptions => _subscriptions;

        public void Dispose()
        {
            DisposeSubscriptions();
            OnDisposed();
        }

        protected virtual void OnDisposed()
        {
        }

        protected void DisposeSubscriptions()
        {
            _subscriptions?.Dispose();
        }
    }
}