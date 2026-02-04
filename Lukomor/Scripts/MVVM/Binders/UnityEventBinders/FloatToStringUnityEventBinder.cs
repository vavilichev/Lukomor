using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class FloatToStringUnityEventBinder : ObservableBinder<float, string>
    {
        [SerializeField] private string _format = "0.00";
        [SerializeField] private UnityEvent<string> _event;

        protected override string HandleValue(float value)
        {
            var result = value.ToString(_format);
            _event.Invoke(result);
            return result;
        }
    }
}