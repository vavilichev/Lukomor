using System.Collections.Generic;
using UnityEngine;

namespace Lukomor.MVVM.PrefabCreation
{
    public class VMCollectionToCreateObjectFromListBinder : ObservableCollectionBinder<IViewModel>
    {
        [SerializeField] private ViewModelToViewMapper _mapper;
        [SerializeField] private Transform _viewsContainer;
        
        private readonly Dictionary<IViewModel, View> _createdViews = new();

        protected override void OnItemAdded(IViewModel viewModel)
        {
            if (_createdViews.ContainsKey(viewModel))
            {
                return;
            }

            var prefab = _mapper.GetPrefab(viewModel.GetType().FullName);
            var createdView = Instantiate(prefab, transform);
            
            _createdViews.Add(viewModel, createdView);
            createdView.Bind(viewModel);
        }

        protected override void OnItemRemoved(IViewModel viewModel)
        {
            if (_createdViews.TryGetValue(viewModel, out var view))
            {
                view.Destroy();
                _createdViews.Remove(viewModel);
            }
        }
        
        
#if UNITY_EDITOR
        private void Reset()
        {
            if (_viewsContainer == null)
            {
                _viewsContainer = transform;
            }
        }
#endif
    }
}