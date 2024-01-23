using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class FloatToTextUnityEventBinder : ObservableBinder<float>
    {
        [SerializeField] private UnityEvent<string> _event;
        
        protected override void OnPropertyChanged(float newValue)
        {
            _event.Invoke(newValue.ToString("0.00"));
        }
    }
}