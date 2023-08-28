using System;
using UnityEngine;

namespace Lukomor.MVVM.PrefabCreation
{
    public class ViewModelCollectionToObjectCreationBinder : ObservableBinder<IViewModel>
    {
        [SerializeField] private ViewsContainer _viewsContainer;

        protected override IDisposable BindInternal(IViewModel viewModel)
        {
            return BindCollection(PropertyName, viewModel, OnViewModelAdded, OnViewModelRemoved);
        }

        private void OnViewModelAdded<T>(T addedViewModel) where T : IViewModel
        {
            _viewsContainer.Add(addedViewModel);
        }
        
        private void OnViewModelRemoved<T>(T removedViewModel) where T : IViewModel
        {
            _viewsContainer.Remove(removedViewModel);
        }
    }
}