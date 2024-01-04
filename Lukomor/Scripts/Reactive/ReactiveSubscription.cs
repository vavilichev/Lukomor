using System;

namespace Lukomor.Reactive
{
    public sealed class ReactiveSubscription<T> : IDisposable
    {
        private IReactiveProperty<T> _propertyOwner;
        private IObserver<T> _observer;

        public ReactiveSubscription(IReactiveProperty<T> propertyOwner, IObserver<T> observer)
        {
            _propertyOwner = propertyOwner;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_propertyOwner == null)
            {
                return;
            }
                
            _propertyOwner.Unsubscribe(_observer);
            _propertyOwner = null;
            _observer = null;
        }
    }
}