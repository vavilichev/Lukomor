using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public class VmToGameObjectCreationFromListBinderDeprecated : ObservableBinderDeprecated<IViewModel>
    {
        [SerializeField] private IViewModelToViewMapper _mapper;

        private View _createdView;

        protected override void OnPropertyChanged(IViewModel newValue)
        {
            // var prefabView = _mapper.GetPrefab(newValue.GetType().FullName);
            // _createdView = Instantiate(prefabView, transform);
                
            _createdView.Bind(newValue);
        }
    }
}