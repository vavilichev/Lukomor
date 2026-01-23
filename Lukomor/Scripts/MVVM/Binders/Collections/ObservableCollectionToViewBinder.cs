using System.Collections.Generic;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public class ObservableCollectionToViewBinder : ObservableCollectionBinder<IViewModel>
    {
        [SerializeField] private ViewModelToViewDirectRefMapper _mapper;
        
        private readonly Dictionary<IViewModel, View> _createdViews = new();

        protected override void OnValueAdded(IViewModel value)
        {
            if (_createdViews.ContainsKey(value))
            {
                return;
            }

            var prefab = _mapper.GetPrefab(value);
            var createdView = Instantiate(prefab, transform);
            
            _createdViews.Add(value, createdView);
            createdView.Bind(value);
        }

        protected override void OnValueRemoved(IViewModel value)
        {
            if (_createdViews.TryGetValue(value, out var view))
            {
                view.Destroy();
                _createdViews.Remove(value);
            }
        }
    }
}