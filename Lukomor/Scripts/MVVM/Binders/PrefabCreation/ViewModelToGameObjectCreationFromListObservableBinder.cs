using UnityEngine;

namespace Lukomor.MVVM.PrefabCreation
{
    public class ViewModelToGameObjectCreationFromListObservableBinder : ObservableBinder<IViewModel>
    {
        [SerializeField] private ViewModelToPrefabMapper _mapper;

        protected override void BindInternal(IViewModel viewModel)
        {
            BindObservable(_propertyName, viewModel, OnViewModelChanged);
        }

        private void OnViewModelChanged(IViewModel newViewModel)
        {
            var modelType = newViewModel.GetType().FullName;
            var prefab = _mapper.GetPrefab(modelType);
            var createdGameObject = Instantiate(prefab, transform);
            var allBinders = createdGameObject.GetComponentsInChildren<IBinder>(true);

            foreach (var binder in allBinders)
            {
                binder.Bind(newViewModel);
            }
        }
    }
}