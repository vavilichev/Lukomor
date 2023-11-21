using Lukomor.MVVM.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class IntToBoolUnityEventBinder : ObservableBinder<int>
    {
        [SerializeField] private CompareOperation _compareOperation;
        [SerializeField] private int _comparingValue;
        
        [SerializeField] private UnityEvent<bool> _event;
		
        protected override void OnPropertyChanged(int newValue)
        {
            var result = _compareOperation.Compare(newValue, _comparingValue);

            _event.Invoke(result);
        }
    }
}