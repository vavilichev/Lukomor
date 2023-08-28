using System.Collections.Generic;
using UnityEngine;

namespace Lukomor.MVVM.PrefabCreation
{
    public class ViewsContainer : MonoBehaviour
    {
        [SerializeField] private View _prefabView;

        private readonly Dictionary<IViewModel, View> _createdViews = new();
        
        public void Add(IViewModel viewModel)
        {
            if (_createdViews.ContainsKey(viewModel))
            {
                return;
            }

            var createdView = Instantiate(_prefabView, transform);
            
            _createdViews.Add(viewModel, createdView);
            createdView.Bind(viewModel);
        }

        public void Remove(IViewModel viewModel)
        {
            if (_createdViews.TryGetValue(viewModel, out var view))
            {
                view.Destroy();
                _createdViews.Remove(viewModel);
            }
        }
    }
}