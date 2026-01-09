using System.Collections.Generic;
using System.Linq;
using Lukomor.MVVM.Binders;
using Lukomor.MVVM.Editor;
using UnityEngine;

namespace Lukomor.MVVM
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class View : MonoBehaviour
    {
        [SerializeField] private View _parentView;

        [SerializeField] private string _viewModelTypeFullName;
        [SerializeField] private string _viewModelPropertyName;
        [SerializeField] private bool _isParentView;

        [SerializeField] private List<View> _subViews = new();
        [SerializeField] private List<Binder> _childBinders = new();

        [SerializeField] private bool _showEditorLogs;

        public string ViewModelTypeFullName => _viewModelTypeFullName;
        public string ViewModelPropertyName => _viewModelPropertyName;

#if UNITY_EDITOR
        private void Start()
        {
            var parentTransform = transform.parent;

            if (parentTransform)
            {
                var parentView = parentTransform.GetComponentInParent<View>();

                if (parentView != null) parentView.RegisterView(this);
            }
        }

        private void OnDestroy()
        {
            var parentTransform = transform.parent;

            if (parentTransform)
            {
                var parentView = parentTransform.GetComponentInParent<View>();

                if (parentView != null) parentView.RemoveView(this);
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

            foreach (var subView in _subViews) subView.Bind(targetViewModel);

            foreach (var binder in _childBinders) binder.Bind(targetViewModel);
        }

        public void Destroy()
        {
            // TODO: make it more flexible
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        public void RegisterBinder(Binder binder)
        {
            if (!_childBinders.Contains(binder)) _childBinders.Add(binder);
        }

        public void RemoveBinder(Binder binder)
        {
            _childBinders.Remove(binder);
        }

        public void ValidateViewModelSetup()
        {
            if (_isParentView) return;

            var allParentViews = gameObject.GetComponentsInParent<View>()
                                           .Where(c => !ReferenceEquals(c, this));
            var isCurrentParentValid = allParentViews.Contains(_parentView);

            if (!isCurrentParentValid)
            {
                _viewModelTypeFullName = null;
                _viewModelPropertyName = null;
                Debug.LogError(
                               $"Parent View is not a parent anymore for View ({gameObject.name}). Do not forget to resetup.",
                               gameObject);
                return;
            }

            var parentViewModelFullName = _parentView.ViewModelTypeFullName;
            if (string.IsNullOrEmpty(parentViewModelFullName))
            {
                Debug.LogError("Parent View Model is not selected. Please select a view model first.",
                               _parentView.gameObject);
                return;
            }

            var parentViewModelType = ViewModelsEditorUtility.ConvertViewModelType(parentViewModelFullName);
            if (parentViewModelType == null)
            {
                Debug.LogError("Parent View Model type Error. Please, check it.", _parentView.gameObject);
                return;
            }

            var currentViewModelPropertyName = _viewModelPropertyName;
            if (!string.IsNullOrEmpty(currentViewModelPropertyName))
            {
                var allParentViewModelProperties = parentViewModelType.GetProperties();
                var allValidProperties =
                    allParentViewModelProperties.Where(p =>
                    {
                        var genericArgument = p.PropertyType.GetGenericArguments().First();
                        return typeof(IViewModel).IsAssignableFrom(genericArgument);
                    });

                if (allValidProperties.All(property => property.Name != currentViewModelPropertyName))
                {
                    _viewModelPropertyName = null;
                    Debug.LogWarning(
                                     $"Parent View Model doesn't have property {currentViewModelPropertyName} anymore. Reset to None",
                                     gameObject);
                    return;
                }
            }
        }

        public bool IsValidSetup()
        {
            foreach (var childBinder in _childBinders)
                if (childBinder == null)
                    return false;

            foreach (var subView in _subViews)
                if (subView == null)
                    return false;

            return true;
        }

        [ContextMenu("Force Fix")]
        public void Fix()
        {
            _childBinders.Clear();
            var allFoundChildBinders = gameObject.GetComponentsInChildren<Binder>(true);
            foreach (var foundChildBinder in allFoundChildBinders)
                if (foundChildBinder.ViewModelTypeFullName == ViewModelTypeFullName)
                    RegisterBinder(foundChildBinder);

            _subViews.Clear();
            var allFoundSubViews = gameObject.GetComponentsInChildren<View>(true);
            foreach (var foundSubView in allFoundSubViews)
            {
                var parentView = foundSubView.GetComponentsInParent<View>()
                                             .FirstOrDefault(c => !ReferenceEquals(c, foundSubView));

                if (parentView == this) RegisterView(foundSubView);
            }
        }

        private void RegisterView(View view)
        {
            if (!_subViews.Contains(view)) _subViews.Add(view);
        }

        private void RemoveView(View view)
        {
            _subViews.Remove(view);
        }

        private void OnTransformParentChanged()
        {
            HandleParentViewModelChanging();
        }
        
        public void HandleParentViewModelChanging()
        {
            var setupExisted = !string.IsNullOrEmpty(_viewModelPropertyName) ||
                               !string.IsNullOrEmpty(_viewModelTypeFullName);
            var parentExisted = _parentView != null;
            var allParentViews = gameObject.GetComponentsInParent<View>()
                                           .Where(c => !ReferenceEquals(c, this)).ToList();
            var anyParentExist = allParentViews.Count > 0;

            if (parentExisted)
            {
                var isParentInHierarchy = allParentViews.Contains(_parentView);
                if (!isParentInHierarchy)
                {
                    // Parent has been lost, lets try to find new parent
                    if (anyParentExist)
                    {
                        // New parent exists, let's define it and reset the setup if it's required
                        _parentView = allParentViews.First();
                        if (setupExisted)
                        {
                            _viewModelPropertyName = null;
                            _viewModelTypeFullName = null;
                            Debug.LogWarning($"View [{gameObject.name}]: parent view has been changed, view settings has been reset. Don't forget to redone the setup.",
                                             gameObject);
                        }
                    }
                    else
                    {
                        // The parent existed, but disappeared
                        _isParentView = true;
                        _parentView = null;

                        if (setupExisted)
                        {
                            _viewModelPropertyName = null;
                            _viewModelTypeFullName = null;
                            Debug.LogWarning($"View [{gameObject.name}]: parent view has been lost, view settings has been reset. Don't forget to redone the setup.",
                                             gameObject);
                        }
                    }
                }
                else
                {
                    // parent is still in hierarchy, but maybe it's setup has been changed
                    if (!string.IsNullOrEmpty(_viewModelPropertyName) &&
                        string.IsNullOrEmpty(_parentView._viewModelTypeFullName))
                    {
                        // current property name exists, but parent view model has been reset, so, we must reset current setup
                        _viewModelPropertyName = null;
                        _viewModelTypeFullName = null;
                        Debug.LogWarning($"View [{gameObject.name}]: parent view has been reset, current view settings has been reset as well. Don't forget to redone the setup.",
                                         gameObject);
                    }
                    else if (!string.IsNullOrEmpty(_viewModelPropertyName))
                    {
                        var parentViewModelTypeFullName = _parentView!.ViewModelTypeFullName;
                        var parentViewModelType =
                            ViewModelsEditorUtility.ConvertViewModelType(parentViewModelTypeFullName);
                        var parentViewModelProperties =
                            ViewModelsEditorUtility.GetAllValidViewModelPropertyNames(parentViewModelType);

                        if (!parentViewModelProperties.Contains(_viewModelPropertyName))
                        {
                            // There is no property in the parent view model. Maybe view model has been changed. Reset
                            Debug.LogWarning($"View [{gameObject.name}]: parent view model has been changed and new view model doesn't have a property with name {_viewModelPropertyName}. The setup has been reset. Don't forget to redone the setup.",
                                             gameObject);
                            _viewModelPropertyName = null;
                            _viewModelTypeFullName = null;
                        }
                    }
                }
            }
            else
            {
                // parent didn't exist
                if (anyParentExist)
                {
                    // parent didn't exist, but appeared
                    _parentView = allParentViews.First();
                    _isParentView = false;

                    if (setupExisted)
                    {
                        // as the setup was done, we should reset it and warn user about this reset
                        _viewModelTypeFullName = null;
                        _viewModelPropertyName = null;
                        Debug.LogWarning($"View [{gameObject.name}]: parent view has appeared, view settings has been reset. Don't forget to redone the setup.",
                                         gameObject);
                    }
                }
            }

            // Also update children
            var subViews = gameObject.GetComponentsInChildren<View>().Where(c => !ReferenceEquals(c, this));
            foreach (var subView in subViews)
            {
                subView.HandleParentViewModelChanging();
            }
        }
#endif
    }
}