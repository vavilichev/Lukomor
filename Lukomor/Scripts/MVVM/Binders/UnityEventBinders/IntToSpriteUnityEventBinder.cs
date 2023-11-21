using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class IntToSpriteUnityEventBinder : ObservableBinder<int>
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

        protected override void OnPropertyChanged(int newValue)
        {
            if (_spritesMap.TryGetValue(newValue, out var sprite))
            {
                _event.Invoke(sprite);
            }
            
            _event.Invoke(_spriteByDefault);
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