using Lukomor.MVVM.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class IntToBoolUnityEventBinder : ObservableBinder<int, bool>
    {
        [SerializeField] private CompareOperation _compareOperation;
        [SerializeField] private int _comparingValue;

        [SerializeField] private UnityEvent<bool> _event;

        protected override bool HandleValue(int value)
        {
            var result = _compareOperation.Compare(value, _comparingValue);

            _event.Invoke(result);

            return result;
        }
    }
}