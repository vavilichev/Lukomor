using UnityEngine;

namespace Lukomor.MVVM.PrefabCreation
{
    public class ViewModelCollectionToObjectCreationBinder : ObservableCollectionBinder<IViewModel>
    {
        [SerializeField] private ViewsContainer _viewsContainer;

        protected override void OnItemAdded(IViewModel value)
        {
            _viewsContainer.Add(value);
        }

        protected override void OnItemRemoved(IViewModel value)
        {
            _viewsContainer.Remove(value);
        }
    }
}