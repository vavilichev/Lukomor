using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lukomor.MVVM.PrefabCreation
{
    public class ViewModelToPrefabMapper : MonoBehaviour
    {
        [Serializable]
        public class ViewModelToPrefabMapping
        {
            // [ViewModelType]
            public string ViewModelType;
            public GameObject Prefab;
        }

        [SerializeField] private List<ViewModelToPrefabMapping> _mappings;

        public GameObject GetPrefab(string viewModelType)
        {
            var entry = _mappings.FirstOrDefault(e => e.ViewModelType == viewModelType);

            return entry?.Prefab;
        }
    }
}