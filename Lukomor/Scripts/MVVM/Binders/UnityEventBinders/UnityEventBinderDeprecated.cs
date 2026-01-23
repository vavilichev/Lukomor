using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public abstract class UnityEventBinderDeprecated<T> : ObservableBinderDeprecated<T>
    {
        [SerializeField] private UnityEvent<T> _event;
        
        protected override void OnPropertyChanged(T newValue)
        {
            _event.Invoke(newValue);
        }
    }
}
