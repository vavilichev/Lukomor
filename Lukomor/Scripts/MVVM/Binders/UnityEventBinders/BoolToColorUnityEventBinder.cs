using System;
using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class BoolToColorUnityEventBinder : ObservableBinder<bool>
    {
        [SerializeField] private Color _colorTrue;
        [SerializeField] private Color _colorFalse;

        [SerializeField] private UnityEvent<Color> _event;
        protected override IDisposable BindInternal(IViewModel viewModel)
        {
            return BindObservable(PropertyName, viewModel, OnValueChanged);
        }

        private void OnValueChanged(bool newValue)
        {
            var color = newValue ? _colorTrue : _colorFalse;
            
            _event.Invoke(color);
        }
    }
}