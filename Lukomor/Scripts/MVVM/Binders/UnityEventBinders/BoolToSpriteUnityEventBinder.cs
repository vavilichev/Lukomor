using System;
using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class BoolToSpriteUnityEventBinder : ObservableBinder<bool>
    {
        [SerializeField] private Sprite _spriteTrue;
        [SerializeField] private Sprite _spriteFalse;

        [SerializeField] private UnityEvent<Sprite> _event;
        protected override IDisposable BindInternal(IViewModel viewModel)
        {
            return BindObservable(PropertyName, viewModel, OnValueChanged);
        }

        private void OnValueChanged(bool newValue)
        {
            var sprite = newValue ? _spriteTrue : _spriteFalse;
            
            _event.Invoke(sprite);
        }
    }
}