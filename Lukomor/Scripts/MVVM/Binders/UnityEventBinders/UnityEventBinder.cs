using System;
using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public abstract class UnityEventBinder<T> : ObservableBinder<T>
    {
        [SerializeField] private UnityEvent<T> _event;
        
        protected override IDisposable BindInternal(IViewModel viewModel)
        {
            return BindObservable(PropertyName, viewModel, OnPropertyChanged);
        }

        private void OnPropertyChanged(T newValue)
        {
            _event.Invoke(newValue);
        }
    }
}
