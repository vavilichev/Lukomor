using System;
using System.Collections.Generic;
using System.Linq;
using Lukomor.MVVM.Binders;
using Lukomor.Reactive;
using UnityEditor;
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
        [SerializeField] private List<BinderDEPRECATED> _childBinders = new();

        [SerializeField] private bool _showEditorLogs;

        private readonly ReactiveProperty<IViewModel> _viewModel = new();
        
        public string ViewModelTypeFullName => _viewModelTypeFullName;
        public string ViewModelPropertyName => _viewModelPropertyName;
        public IObservable<IViewModel> ViewModel => _viewModel;

#if UNITY_EDITOR
        
        private void OnEnable()
        {
            if (!EditorApplication.isPlaying)
            {
                if (_parentView == null)
                {
                    var potentialParentView = this.FirstOrDefaultParentView();
                    UpdateParentViewRegistration(potentialParentView);
                }
                else
                {
                    UpdateParentViewRegistration(_parentView);
                }
            }
        }

        private void OnDestroy()
        {
            RemoveWarningIcon();
            _parentView?.RemoveViewRegistration(this);
        }

        private void OnValidate()
        {
            CheckForWarningIcon();
        }

        private void Reset()
        {
            ValidateViewModelSetup();
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
        public void RegisterBinder(BinderDEPRECATED binderDeprecated)
        {
            if (!_childBinders.Contains(binderDeprecated)) _childBinders.Add(binderDeprecated);
        }

        public void RemoveBinder(BinderDEPRECATED binderDeprecated)
        {
            _childBinders.Remove(binderDeprecated);
        }

        public void ValidateViewModelSetup()
        {
            var allParentViews = this.AllParentViews();
            var isParentViewExist = _parentView != null;
            var shouldParentViewExist = allParentViews.Any();
            
            if (isParentViewExist)
            {
                var setupExist = !string.IsNullOrEmpty(_viewModelPropertyName) && !string.IsNullOrEmpty(_viewModelTypeFullName);
                if (!setupExist)
                {
                    ShowWarningIcon();
                    return;
                }
                
                RemoveWarningIcon();
            }
            else
            {
                // no parent view
                _viewModelPropertyName = null;  // property name must by null in case of root view model
                
                if (shouldParentViewExist)
                {
                    // no parent view but should exist
                    ShowWarningIcon();
                    return;
                }

                RemoveWarningIcon();
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
            var allFoundChildBinders = gameObject.GetComponentsInChildren<BinderDEPRECATED>(true);
            foreach (var foundChildBinder in allFoundChildBinders)
                if (foundChildBinder.ViewModelTypeFullName == ViewModelTypeFullName)
                    RegisterBinder(foundChildBinder);

            _subViews.Clear();
            var allFoundSubViews = gameObject.GetComponentsInChildren<View>(true);
            foreach (var foundSubView in allFoundSubViews)
            {
                var parentView = foundSubView.FirstOrDefaultParentView();
                if (parentView == this) RegisterView(foundSubView);
            }
        }

        public void RegisterView(View view)
        {
            if (!_subViews.Contains(view)) _subViews.Add(view);
        }

        public void RemoveViewRegistration(View view)
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
            var allParentViews = this.AllParentViews().ToList();
            var anyParentExist = allParentViews.Count > 0;

            if (parentExisted)
            {
                var isParentInHierarchy = allParentViews.Contains(_parentView);
                if (!isParentInHierarchy)
                {
                    _parentView.RemoveViewRegistration(this);
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
                                Editor.ViewModelsEditorUtility.ConvertViewModelType(parentViewModelTypeFullName);
                            if (!Editor.ViewModelsEditorUtility.DoesViewModelHaveProperty(parentViewModelType,
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
            ValidateViewModelSetup();

            // Also update children
            foreach (var subView in _subViews)
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
                    ShowWarningIcon();
                }
                else
                {
                    RemoveWarningIcon();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_viewModelPropertyName))
                {
                    ShowWarningIcon();
                }
                else
                {
                    RemoveWarningIcon();
                }
            }
        }

        private void UpdateParentViewRegistration(View newParentView)
        {
            _parentView?.RemoveViewRegistration(this);
            _parentView = newParentView;
            newParentView?.RegisterView(this);
        }

        private void ShowWarningIcon()
        {
            Editor.WarningIconDrawer.AddWarningView(this.GetId());
            _parentView?.ShowWarningIcon();
        }

        private void RemoveWarningIcon()
        {
            Editor.WarningIconDrawer.RemoveWarningView(this.GetId());
            _parentView?.CheckForWarningIcon();
            _parentView?.ValidateViewModelSetup();
        }

        private void LogWarn(string message)
        {
            Debug.LogWarning(message, gameObject);
        }
#endif
    }
}