using UnityEngine;

namespace Lukomor.MVVM.PrefabCreation
{
    public class ViewModelToGameObjectCreation : Binder<IViewModel>
    {
        [SerializeField] private GameObject _prefab;

        protected override void BindInternal(IViewModel viewModel)
        {
            BindLikeElement(_propertyName, viewModel, OnViewModelChanged);
        }

        private void OnViewModelChanged(IViewModel newViewModel)
        {
            var createdGameObject = Instantiate(_prefab, transform);

            TakeViewModelToChildBinders(createdGameObject, newViewModel);
        }
    }
}