using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class IntToColorUnityEventBinder : ObservableBinder<int, Color>
    {
        [SerializeField] private List<IntToColorMapping> _mappings = new();
        [SerializeField] private Color _colorByDefault = Color.white;

        [SerializeField] private UnityEvent<Color> _event;

        private Dictionary<int, Color> _colorsMap;

        private void Awake()
        {
            _colorsMap = new Dictionary<int, Color>();

            foreach (var mapping in _mappings)
            {
                _colorsMap.Add(mapping.Value, mapping.Color);
            }
        }

        protected override Color HandleValue(int value)
        {
            if (_colorsMap.TryGetValue(value, out var color))
            {
                _event.Invoke(color);
                return color;
            }

            _event.Invoke(_colorByDefault);
            return _colorByDefault;
        }
    }

    [Serializable]
    public class IntToColorMapping
    {
        [SerializeField] private int _value;
        [SerializeField] private Color _color = Color.white;

        public int Value => _value;
        public Color Color => _color;
    }
}