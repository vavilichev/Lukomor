using System;
using System.Collections.Generic;

namespace Lukomor.Reactive
{
    public class SingleReactiveProperty<T> : IReactiveProperty<T>
    {
        private T _value;
        private bool _hasValue;
        private IObserver<T> _observer;

        public T Value
        {
            get => _value;
            set
            {
                if (!_hasValue)
                {
                    _hasValue = true;
                    _value = value;

                    _observer?.OnNext(value);
                }
                else if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;

                    _observer?.OnNext(value);
                }
            }
        }
        
        public bool HasValue => _hasValue;

        public SingleReactiveProperty() { }

        public SingleReactiveProperty(T valueByDefault)
        {
            _value = valueByDefault;
            _hasValue = true;
        }
        
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (_observer != null)
            {
                throw new Exception("You cannot subscribe on SingleReactiveProperty more than one time");
            }

            _observer = observer;

            if (_hasValue)
            {
                observer.OnNext(_value);
            }

            return new ReactiveSubscription<T>(this, observer);
        }
        
        public void Unsubscribe(IObserver<T> observer)
        {
            _observer = null;
        }
    }
}