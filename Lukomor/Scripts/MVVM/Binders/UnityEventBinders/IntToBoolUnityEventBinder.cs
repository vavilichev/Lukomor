using System;
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
		
        protected override IDisposable BindInternal(IViewModel viewModel)
        {
            return BindObservable(PropertyName, viewModel, OnActiveObjectsAmountChanged);
        }

        private void OnActiveObjectsAmountChanged(int receivedAmount)
        {
            var result = _compareOperation.Compare(receivedAmount, _comparingValue);

            _event.Invoke(result);
        }
    }
}