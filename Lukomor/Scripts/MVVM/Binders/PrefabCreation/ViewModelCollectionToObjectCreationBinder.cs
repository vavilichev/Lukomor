using System.Collections.Generic;
using UnityEngine;

namespace Lukomor.MVVM.PrefabCreation
{
    public class ViewModelCollectionToObjectCreationBinder : Binder<IViewModel>
    {
        [SerializeField] private GameObject _prefab;

        private Dictionary<IViewModel, GameObject> _createdObjects = new();

        protected override void BindInternal(IViewModel viewModel)
        {
            BindLikeCollection(_propertyName, viewModel, OnViewModelAdded, OnViewModelRemoved);
        }

        private void OnViewModelAdded<T>(T newViewModel) where T : IViewModel
        {
            var createdGameObject = Instantiate(_prefab, transform);
            _createdObjects[newViewModel] = createdGameObject;

            TakeViewModelToChildBinders(createdGameObject, newViewModel);
        }
        
        private void OnViewModelRemoved<T>(T removedViewModel) where T : IViewModel
        {
            if (_createdObjects.TryGetValue(removedViewModel, out var go))
            {
                Destroy(go);

                _createdObjects.Remove(removedViewModel);
            }
        }
    }
}