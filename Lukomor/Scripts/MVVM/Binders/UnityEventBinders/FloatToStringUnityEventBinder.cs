using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class FloatToStringUnityEventBinder : ObservableBinder<float, string>
    {
        [SerializeField] private UnityEvent<string> _event;

        protected override string HandleValue(float value)
        {
            var result = value.ToString("0.00");
            _event.Invoke(result);
            return result;
        }
    }
}