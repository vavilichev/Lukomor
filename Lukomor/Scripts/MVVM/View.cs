using System.Collections.Generic;
using UnityEngine;

namespace Lukomor.MVVM
{
    public class View : MonoBehaviour
    {
        [SerializeField] private string _viewModelTypeFullName;

        [SerializeField] private List<View> _subViews = new();
        [SerializeField] private List<Binder> _childBinders = new();
        
        public void Bind(IViewModel viewModel)
        {
            foreach (var subView in _subViews)
            {
                var subViewModel = default(IViewModel); // TODO: Get SubViewModel from viewModel
                
                subView.Bind(subViewModel);
            }
            
            foreach (var binder in _childBinders)
            {
                binder.Bind(viewModel);
            }
        }

        public void Destroy()
        {
            // TODO: make it more flexible
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        public void RegisterBinder(Binder binder)
        {
            if (!_childBinders.Contains(binder))
            {
                _childBinders.Add(binder);
            }
        }

        public void RemoveBinder(Binder binder)
        {
            _childBinders.Remove(binder);
        }
#endif
        
    }
}