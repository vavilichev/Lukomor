using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class IntToSpriteUnityEventBinder : ObservableBinder<int, Sprite>
    {
        [SerializeField] private List<IntToSpriteMapping> _mappings = new();
        [SerializeField] private Sprite _spriteByDefault;

        [SerializeField] private UnityEvent<Sprite> _event;

        private Dictionary<int, Sprite> _spritesMap;

        private void Awake()
        {
            _spritesMap = new Dictionary<int, Sprite>();

            foreach (var mapping in _mappings)
            {
                _spritesMap.Add(mapping.Value, mapping.Sprite);
            }
        }

        protected override Sprite HandleValue(int value)
        {
            if (_spritesMap.TryGetValue(value, out var sprite))
            {
                _event.Invoke(sprite);
                return sprite;
            }

            _event.Invoke(_spriteByDefault);
            return _spriteByDefault;
        }
    }

    [Serializable]
    public class IntToSpriteMapping
    {
        [SerializeField] private int _value;
        [SerializeField] private Sprite _sprite;

        public int Value => _value;
        public Sprite Sprite => _sprite;
    }
}