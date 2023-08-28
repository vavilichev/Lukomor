using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lukomor.MVVM.PrefabCreation
{
    public class ViewModelToViewMapper : MonoBehaviour
    {
        [Serializable]
        public class ViewModelToViewMapping
        {
            public string ViewModelTypeFullName;
            public View PrefabView;
        }

        [SerializeField] private List<ViewModelToViewMapping> _prefabMappings;

        private readonly Dictionary<string, View> _mappings = new();

        private void Awake()
        {
            foreach (var prefabMapping in _prefabMappings)
            {
                _mappings.TryAdd(prefabMapping.ViewModelTypeFullName, prefabMapping.PrefabView);
            }
        }

        public View GetPrefab(string viewModelTypeFullName)
        {
            return _mappings[viewModelTypeFullName];
        }
    }
}