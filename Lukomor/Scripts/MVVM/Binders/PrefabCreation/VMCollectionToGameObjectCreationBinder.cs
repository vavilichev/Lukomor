using System.Collections.Generic;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public class VMCollectionToGameObjectCreationBinder : ObservableCollectionBinder<IViewModel>
    {
        [SerializeField] private View _prefabView;

        private readonly Dictionary<IViewModel, View> _createdViews = new();
        
        protected override void OnItemAdded(IViewModel value)
        {
            if (_createdViews.ContainsKey(value))
            {
                return;
            }

            var createdView = Instantiate(_prefabView, transform);
            
            _createdViews.Add(value, createdView);
            createdView.Bind(value);
        }

        protected override void OnItemRemoved(IViewModel value)
        {
            if (_createdViews.TryGetValue(value, out var view))
            {
                view.Destroy();
                _createdViews.Remove(value);
            }
        }
    }
}