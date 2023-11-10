﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class IntToColorUnityEventBinder : ObservableBinder<int>
    {
        [SerializeField] private IntToColorMapping[] _mappings;
        [SerializeField] private Color _colorByDefault;
        
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

        protected override IDisposable BindInternal(IViewModel viewModel)
        {
            return BindObservable(PropertyName, viewModel, OnValueChanged);
        }

        private void OnValueChanged(int value)
        {
            if (_colorsMap.TryGetValue(value, out var color))
            {
                _event.Invoke(color);
            }
            
            _event.Invoke(_colorByDefault);
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