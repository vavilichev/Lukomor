using UnityEngine;

namespace Lukomor.MVVM.PrefabCreation
{
    public class ViewModelToGameObjectCreationFromListObservableBinder : ObservableBinder<IViewModel>
    {
        [SerializeField] private ViewModelToViewMapper _mapper;

        private View _createdView;

        protected override void OnPropertyChanged(IViewModel newValue)
        {
            var prefabView = _mapper.GetPrefab(newValue.GetType().FullName);
            var createdView = Instantiate(prefabView, transform);
                
            createdView.Bind(newValue);
        }
    }
}