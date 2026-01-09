using System.Collections.Generic;
using System.Linq;
using Lukomor.MVVM.Binders;
using Lukomor.MVVM.Editor;
using UnityEditor.SceneManagement;
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
            var allParentViews = gameObject.GetComponentsInParent<View>().Where(v => !ReferenceEquals(v, this));
            var isParentViewExist = _parentView != null;
            var shouldParentViewExist = allParentViews.Any();
            
            if (isParentViewExist)
            {
                var setupExist = !string.IsNullOrEmpty(_viewModelPropertyName) && !string.IsNullOrEmpty(_viewModelTypeFullName);
                if (!setupExist)
                {
                    WarningIconDrawer.AddWarningView(gameObject.GetInstanceID());
                    return;
                }
                
                WarningIconDrawer.RemoveWarningView(gameObject.GetInstanceID());
            }
            else
            {
                // no parent view
                _viewModelPropertyName = null;  // property name must by null in case of root view model
                
                if (shouldParentViewExist)
                {
                    // no parent view but should exist
                    WarningIconDrawer.AddWarningView(gameObject.GetInstanceID());
                    return;
                }

                WarningIconDrawer.RemoveWarningView(gameObject.GetInstanceID());
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
                    _parentView = null;
                    _isParentView = !anyParentExist;

                    // Parent has been lost
                    if (anyParentExist)
                    {
                        // new parent exists, but we will clean the parent view and remain viewmodel type as it was to keep children setup valid, and warn user that parent is abscent
                        LogWarn($"View [{gameObject.name}]: parent view has been lost, you have to specify parent view for sub view otherwise the setup will not work.");
                    }
                    else
                    {
                        // new parent doesn't exist
                        LogWarn($"View [{gameObject.name}]: parent view has been lost, view became a root view. Make sure that your setup is still valid.");
                    }
                }
                else
                {
                    // parent is still in hierarchy, but maybe it's setup has been changed

                    if (!string.IsNullOrEmpty(_viewModelPropertyName))
                    {
                        var parentViewModelTypeFullName = _parentView?._viewModelTypeFullName;
                        if (string.IsNullOrEmpty(parentViewModelTypeFullName))
                        {
                            _viewModelPropertyName = null;
                            LogWarn($"View [{gameObject.name}]: parent view doesn't have view model type. Couldn't keep the property name, that's why it has been reset. Don't forget to setup View");
                        }
                        else
                        {
                            var parentViewModelType =
                                ViewModelsEditorUtility.ConvertViewModelType(parentViewModelTypeFullName);
                            if (!ViewModelsEditorUtility.DoesViewModelHaveProperty(parentViewModelType,
                                    _viewModelPropertyName))
                            {
                                LogWarn($"View [{gameObject.name}]: parent view model doesn't have property with name ({_viewModelPropertyName}). Couldn't keep the property name, that's why it has been reset. Don't forget to setup View");
                                _viewModelPropertyName = null;
                            }
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
                    _isParentView = false;
                    // _viewModelPropertyName = null;
                    if (setupExisted)
                    {
                        LogWarn($"View [{gameObject.name}]: Found parent Views. Don't forget to setup View otherwise it will not work.");
                    }
                }
            }
            
            CheckForWarningIcon();

            // Also update children
            var subViews = gameObject.GetComponentsInChildren<View>().Where(c => !ReferenceEquals(c, this));
            foreach (var subView in subViews)
            {
                subView.HandleParentViewModelChanging();
            }
        }
        
        public void CheckForWarningIcon()
        {
            var isInPrefabMode = PrefabStageUtility.GetPrefabStage(gameObject);
            
            if (_isParentView)
            {
                // warning for this case can only exist for not a prefab mode
                if (string.IsNullOrEmpty(_viewModelTypeFullName) && !isInPrefabMode)
                {
                    WarningIconDrawer.AddWarningView(gameObject.GetInstanceID());
                }
                else
                {
                    WarningIconDrawer.RemoveWarningView(gameObject.GetInstanceID());
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_viewModelPropertyName))
                {
                    WarningIconDrawer.AddWarningView(gameObject.GetInstanceID());
                }
                else
                {
                    WarningIconDrawer.RemoveWarningView(gameObject.GetInstanceID());
                }
            }
        }

        private void LogWarn(string message)
        {
            Debug.LogWarning(message, gameObject);
        }
#endif
    }
}