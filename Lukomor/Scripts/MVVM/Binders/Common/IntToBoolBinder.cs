using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public class IntToBoolBinder : ObservableBinder<int, bool>
    {
        [SerializeField] private IntToBoolMapping[] _mappings;
        [SerializeField] private bool _valueByDefault;

        private readonly Dictionary<int, bool> _mappingsMap = new();

        private void Awake()
        {
            foreach (var mapping in _mappings)
            {
                _mappingsMap.Add(mapping.Key, mapping.Value);
            }
        }

        protected override bool HandleValue(int value)
        {
            return _mappingsMap.GetValueOrDefault(value, _valueByDefault);
        }

        [Serializable]
        public class IntToBoolMapping
        {
            public int Key;
            public bool Value;
        }
    }
}