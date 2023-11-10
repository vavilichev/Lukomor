using System;
using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class DoubleToTextUnityEventBinder : ObservableBinder<double>
    {
        [SerializeField] private UnityEvent<string> _event;
        
        protected override IDisposable BindInternal(IViewModel viewModel)
        {
            return BindObservable(PropertyName, viewModel, OnPropertyChanged);
        }
        
        private void OnPropertyChanged(double newValue)
        {
            _event.Invoke(newValue.ToString("0.00"));
        }
    }
}