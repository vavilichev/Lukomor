using System.Collections.Generic;
using System.Linq;
using Lukomor.MVVM.Binders;
using UnityEngine;

namespace Lukomor.MVVM
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class View : MonoBehaviour
    {
        [SerializeField] private string _viewModelTypeFullName;
        [SerializeField] private string _viewModelPropertyName;
        [SerializeField] private bool _isParentView;

        [SerializeField] private List<View> _subViews = new();
        [SerializeField] private List<Binder> _childBinders = new();

        public string ViewModelTypeFullName => _viewModelTypeFullName;
        public string ViewModelPropertyName => _viewModelPropertyName;

#if UNITY_EDITOR
        private void Start()
        {
            var parentTransform = transform.parent;

            if (parentTransform)
            {
                var parentView = parentTransform.GetComponentInParent<View>();

                if (parentView != null)
                {
                    parentView.RegisterView(this);
                }
            }
        }
        
        private void OnDestroy()
        {
            var parentTransform = transform.parent;

            if (parentTransform)
            {
                var parentView = parentTransform.GetComponentInParent<View>();

                if (parentView != null)
                {
                    parentView.RemoveView(this);
                }
            }
        }
#endif
        
        public void Bind(IViewModel viewModel)
        {
            IViewModel targetViewModel;
            
            if (_isParentView)
            {
                targetViewModel = viewModel;
            }
            else
            {
                var property = viewModel.GetType().GetProperty(_viewModelPropertyName);
                targetViewModel = (IViewModel)property.GetValue(viewModel);
            }
            
            foreach (var subView in _subViews)
            {
                subView.Bind(targetViewModel);
            }
            
            foreach (var binder in _childBinders)
            {
                binder.Bind(targetViewModel);
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

        public bool IsValidSetup()
        {
            foreach (var childBinder in _childBinders)
            {
                if (childBinder == null)
                {
                    return false;
                }
            }

            foreach (var subView in _subViews)
            {
                if (subView == null)
                {
                    return false;
                }
            }

            return true;
        }

        [ContextMenu("Force Fix")]
        public void Fix()
        {
            _childBinders.Clear();
            var allFoundChildBinders = gameObject.GetComponentsInChildren<Binder>(true);
            foreach (var foundChildBinder in allFoundChildBinders)
            {
                if (foundChildBinder.ViewModelTypeFullName == ViewModelTypeFullName)
                {
                    RegisterBinder(foundChildBinder);
                }
            }

            _subViews.Clear();
            var allFoundSubViews = gameObject.GetComponentsInChildren<View>(true);
            foreach (var foundSubView in allFoundSubViews)
            {
                var parentView = foundSubView.GetComponentsInParent<View>().FirstOrDefault(c => !ReferenceEquals(c, foundSubView));
            
                if (parentView == this)
                {
                    RegisterView(foundSubView);
                }
            }

        }

        private void RegisterView(View view)
        {
            if (!_subViews.Contains(view))
            {
                _subViews.Add(view);
            }
        }

        private void RemoveView(View view)
        {
            _subViews.Remove(view);
        }
#endif
    }
}