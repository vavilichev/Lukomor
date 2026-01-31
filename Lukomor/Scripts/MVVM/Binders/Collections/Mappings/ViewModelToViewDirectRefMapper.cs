using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    /// <summary>
    /// This mapper contains direct links in the inspector: ViewModel to View prefab.
    /// This mapper has a custom inspector to have user-friendly mapping for ViewModel.
    /// </summary>
    public class ViewModelToViewDirectRefMapper : ViewModelToViewBaseMapper
    {
        [SerializeField] private ViewModelToViewDirectRefMapping[] _mappings;
        
        private readonly Dictionary<string, View> _prefabsMap = new();

        public override void Init()
        {
            foreach (var mapping in _mappings)
            {
                _prefabsMap[mapping.ViewModelFullTypeName] = mapping.Prefab;
            }
        }

        public override View GetPrefab(IViewModel viewModel)
        {
            var viewModelTypeFullname = viewModel.GetType().FullName;
            return _prefabsMap[viewModelTypeFullname!];
        }

        public override Task<View> GetPrefabAsync(IViewModel viewModel)
        {
            var viewModelTypeFullname = viewModel.GetType().FullName;
            return Task.FromResult(_prefabsMap[viewModelTypeFullname!]);
        }
    }
}