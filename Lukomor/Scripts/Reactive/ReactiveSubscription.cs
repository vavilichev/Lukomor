using System;

namespace Lukomor.Reactive
{
    public sealed class ReactiveSubscription<T> : IDisposable
    {
        private IReactiveProperty<T> _property;
        private IObserver<T> _observer;

        public ReactiveSubscription(IReactiveProperty<T> property, IObserver<T> observer)
        {
            _property = property;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_property == null)
            {
                return;
            }
                
            _property.Unsubscribe(_observer);
            _property = null;
            _observer = null;
        }
    }
}