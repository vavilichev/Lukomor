using System;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    [Serializable]
    public class ViewModelToViewDirectRefMapping
    {
        [SerializeField] private string _viewModelFullTypeName;
        [SerializeField] private View _prefab;
        
        public string ViewModelFullTypeName => _viewModelFullTypeName;
        public View Prefab => _prefab;
    }
}