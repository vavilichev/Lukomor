using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class DoubleToTextUnityEventBinderDeprecated : ObservableBinderDeprecated<double>
    {
        [SerializeField] private string _format = "0.00";
        [SerializeField] private UnityEvent<string> _event;
        
        protected override void OnPropertyChanged(double newValue)
        {
            _event.Invoke(newValue.ToString(_format));
        }
    }
}