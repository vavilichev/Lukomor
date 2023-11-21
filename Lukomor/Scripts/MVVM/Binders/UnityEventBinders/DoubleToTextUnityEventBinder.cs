using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class DoubleToTextUnityEventBinder : ObservableBinder<double>
    {
        [SerializeField] private UnityEvent<string> _event;
        
        protected override void OnPropertyChanged(double newValue)
        {
            _event.Invoke(newValue.ToString("0.00"));
        }
    }
}