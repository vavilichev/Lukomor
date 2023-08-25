using UnityEngine;

namespace Lukomor.MVVM.PrefabCreation
{
    public class ViewModelToGameObjectCreation : ObservableBinder<IViewModel>
    {
        [SerializeField] private GameObject _prefab;

        protected override void BindInternal(IViewModel viewModel)
        {
            BindObservable(_propertyName, viewModel, OnViewModelChanged);
        }

        private void OnViewModelChanged(IViewModel newViewModel)
        {
            var createdGameObject = Instantiate(_prefab, transform);

            TakeViewModelToChildBinders(createdGameObject, newViewModel);
        }
    }
}