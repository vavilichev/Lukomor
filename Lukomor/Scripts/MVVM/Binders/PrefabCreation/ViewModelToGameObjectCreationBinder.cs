using UnityEngine;

namespace Lukomor.MVVM.PrefabCreation
{
    public class ViewModelToGameObjectCreationBinder : ObservableBinder<IViewModel>
    {
        [SerializeField] private View _prefabView;
        
        protected override void OnPropertyChanged(IViewModel newValue)
        {
            var createdView = Instantiate(_prefabView, transform);
                
            createdView.Bind(newValue);
        }
    }
}