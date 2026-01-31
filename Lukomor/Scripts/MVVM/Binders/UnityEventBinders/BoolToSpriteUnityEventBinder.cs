using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class BoolToSpriteUnityEventBinder : ObservableBinder<bool, Sprite>
    {
        [SerializeField] private Sprite _spriteTrue;
        [SerializeField] private Sprite _spriteFalse;

        [SerializeField] private UnityEvent<Sprite> _event;

        protected override Sprite HandleValue(bool value)
        {
            var sprite = value ? _spriteTrue : _spriteFalse;

            _event.Invoke(sprite);

            return sprite;
        }
    }
}