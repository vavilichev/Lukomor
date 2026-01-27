using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class DoubleToStringUnityEventBinder : ObservableBinder<double, string>
    {
        [SerializeField] private string _format = "0.00";
        [SerializeField] private UnityEvent<string> _event;

        protected override string HandleValue(double value)
        {
            var result = value.ToString(_format);

            _event.Invoke(result);

            return result;
        }
    }
}