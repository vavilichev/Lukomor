using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public class VmToGameObjectCreationBinderDeprecated : ObservableBinderDeprecated<IViewModel>
    {
        [SerializeField] private View _prefabView;
        
        protected override void OnPropertyChanged(IViewModel newValue)
        {
            var createdView = Instantiate(_prefabView, transform);
                
            createdView.Bind(newValue);
        }
    }
}