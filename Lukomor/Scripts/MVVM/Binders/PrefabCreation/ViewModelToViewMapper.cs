using System.Collections.Generic;
using Lukomor.MVVM.ViewModels;
using UnityEngine;

namespace Lukomor.MVVM.PrefabCreation
{
    public class ViewModelToViewMapper : MonoBehaviour
    {
        [SerializeField] private List<ViewModelToViewMapping> _prefabMappings;
        [SerializeField] private ViewModelToViewMapping _mapping;

        private readonly Dictionary<string, View> _mappings = new();

        private void Awake()
        {
            foreach (var prefabMapping in _prefabMappings)
            {
                _mappings.TryAdd(prefabMapping.ViewModelTypeFullName, prefabMapping.PrefabView);
            }

            var type = typeof(TestViewModelOlolo);
            var typeFullName = type.FullName;
            var prefab = GetPrefab(typeFullName);
            Debug.Log($"Prefab result: {prefab} ({prefab.name})");
        }

        public View GetPrefab(string viewModelTypeFullName)
        {
            return _mappings[viewModelTypeFullName];
        }
    }
}