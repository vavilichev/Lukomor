using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class IntToTextUnityEventBinder : ObservableBinder<int>
    {
        [SerializeField] private UnityEvent<string> _event;
        
        protected override void OnPropertyChanged(int newValue)
        {
            _event.Invoke(newValue.ToString());
        }
    }
}