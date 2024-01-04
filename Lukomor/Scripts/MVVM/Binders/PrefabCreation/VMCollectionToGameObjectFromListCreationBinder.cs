using System.Collections.Generic;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public class VMCollectionToGameObjectFromListCreationBinder : ObservableCollectionBinder<IViewModel>
    {
        [SerializeField] private ViewModelToViewMapper _mapper;
        
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
    }
}