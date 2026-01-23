using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public class ViewModelToViewDirectRefMapper : MonoBehaviour, IViewModelToViewMapper
    {
        [SerializeField] private ViewModelToViewDirectRefMapping[] _mappings;
        
        private readonly Dictionary<string, View> _prefabsMap = new();

        public void Init()
        {
            foreach (var mapping in _mappings)
            {
                _prefabsMap[mapping.ViewModelFullTypeName] = mapping.Prefab;
            }
        }

        public View GetPrefab(IViewModel viewModel)
        {
            var viewModelTypeFullname = viewModel.GetType().FullName;
            return _prefabsMap[viewModelTypeFullname!];
        }

        public Task<View> GetPrefabAsync(IViewModel viewModel)
        {
            var viewModelTypeFullname = viewModel.GetType().FullName;
            return Task.FromResult(_prefabsMap[viewModelTypeFullname!]);
        }
    }
}