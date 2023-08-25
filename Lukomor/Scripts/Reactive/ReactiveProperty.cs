using System;
using System.Collections.Generic;

namespace Lukomor.Reactive
{
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private List<Action<T>> _callbacks = new();

        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    
                    Notify(_value);
                }
            }
        }

        private T _value;
        
        public ReactiveProperty()
        {
            _value = default;
        }
        
        public ReactiveProperty(T initialValue)
        {
            _value = initialValue;
        }

        public void AddListener(Action<T> callback)
        {
            _callbacks.Add(callback);
            callback?.Invoke(Value);
        }

        public void RemoveListener(Action<T> callback)
        {
            _callbacks.Remove(callback);
        }

        private void Notify(T value)
        {
            foreach (var callback in _callbacks)
            {
                callback?.Invoke(value);
            }
        }
    }
}