using System.Collections.Generic;
using System.Linq;
using Lukomor.MVVM.Binders;
using UnityEditor;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    public static class MVVMValidator
    {
        private static readonly HashSet<int> _errorObjects = new();

        public static void RequestValidation()
        {
            
        }

        [MenuItem("Lukomor/Test Validation")]
        public static void Validate()
        {
            ValidateViews();
            ValidateBinders();
        }

        private static void ValidateViews()
        {
            var allSceneViews = Object.FindObjectsByType<View>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            var allBinders =
                Object.FindObjectsByType<BinderBase>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var view in allSceneViews)
            {
                if (IsBrokenView(view))
                {
                    MarkSelf(view);
                    MarkParents(view);
                    MarkDependedViews(view, allSceneViews);
                    MarkDependedBinders(view, allBinders);
                }
            }

            foreach (var binder in allBinders)
            {
                if (IsBrokenBinder(binder))
                {
                    MarkSelf(binder);
                    MarkParents(binder);
                    MarkDependedBinders(binder, allBinders);
                }
            }
            
        }

        private static bool IsBrokenView(View view)
        {
            var isRootView = view.ParentView == null;
            if (isRootView)
            {
                var isViewModelSelected = !string.IsNullOrEmpty(view.ViewModelTypeFullName);
                if (!isViewModelSelected)
                {
                    view.ResetPropertyName();
                    Debug.LogWarning($"View is broken ({view.gameObject.name}). View Model wasn't selected.", view.gameObject);
                    return true;
                }

                var selectedViewModelType = ViewModelsEditorUtility.ConvertViewModelType(view.ViewModelTypeFullName);
                if (selectedViewModelType == null)
                {
                    // corrupted view model
                    view.ResetPropertyName();
                    view.ResetViewModelFullTypeName();
                    Debug.LogWarning($"View is broken ({view.gameObject.name}). Selected View Model Type is missing.", view.gameObject);
                    return true;
                }

                return false;
            }

            var parentView = view.ParentView;
            var parentViewModelFullTypeName = parentView.ViewModelTypeFullName;
            var parentViewModelType = ViewModelsEditorUtility.ConvertViewModelType(parentViewModelFullTypeName);
            var isParentViewModelValid = parentViewModelType != null;
            if (!isParentViewModelValid)
            {
                // parent viewModel is not valid
                Debug.LogWarning($"Parent view is broken ({view.gameObject.name}). Selected View Model Type is missing.", view.gameObject);
                view.ResetPropertyName();
                return true;
            }
            
            var currentViewModelPropertyName = view.ViewModelPropertyName;
            var isPropertyNameExist = !string.IsNullOrEmpty(currentViewModelPropertyName);
            if (!isPropertyNameExist)
            {
                // property is not selected
                Debug.LogWarning($"PropertyName is not selected in ({view.gameObject.name}).", view.gameObject);
                return true;
            }
            
            var allParentProperties = parentViewModelType.GetProperties();
            var allValidProperties =
                ViewModelsEditorUtility.FilterValidViewModelProperties(allParentProperties, typeof(IViewModel));
            var isCurrentViewModelPropertyNameValid =
                allValidProperties.FirstOrDefault(p => p.Name == currentViewModelPropertyName) != null;

            if (!isCurrentViewModelPropertyNameValid)
            {
                // parent view model corrupted
                view.ResetPropertyName();
                MarkSelf(parentView);
                Debug.LogWarning($"PropertyName is not valid in ({view.gameObject.name}).", view.gameObject);
                return true;
            }

            return false;
        }

        private static bool IsBrokenBinder(BinderBase binder)
        {
            return binder.IsBroken();
        }

        private static void MarkSelf(MonoBehaviour monoBehaviour)
        {
            _errorObjects.Add(monoBehaviour.gameObject.GetInstanceID());
            Debug.Log($"{monoBehaviour.gameObject.name} marked as error by itself", monoBehaviour.gameObject);
        }

        private static void MarkParents(MonoBehaviour monoBehaviour)
        {
            var parentTransform = monoBehaviour.transform.parent;

            while (parentTransform != null)
            {
                _errorObjects.Add(parentTransform.GetInstanceID());
                Debug.Log($"{monoBehaviour.gameObject.name} marked as error as a parent", parentTransform.gameObject);

                parentTransform = parentTransform.parent;
            }
        }

        private static void MarkDependedViews(View sourceView, View[] allViews)
        {
            var allDependedViews = allViews.Where(v => ReferenceEquals(v.ParentView, sourceView));
            foreach (var dependedView in allDependedViews)
            {
                _errorObjects.Add(dependedView.gameObject.GetInstanceID());
                dependedView.SmartReset();
                Debug.Log($"{dependedView.gameObject.name} marked as error as depended View", dependedView.gameObject);
            }
        }
        
        private static void MarkDependedBinders(View sourceView, BinderBase[] allBinders)
        {
            var allDependedBinders = allBinders.Where(b => ReferenceEquals(b.SourceView, sourceView));
            foreach (var dependedBinder in allDependedBinders)
            {
                _errorObjects.Add(dependedBinder.gameObject.GetInstanceID());
                dependedBinder.SmartReset();
                Debug.Log($"{dependedBinder.gameObject.name} marked as error as depended Binder", dependedBinder.gameObject);
            }
        }

        private static void MarkDependedBinders(BinderBase sourceBinder, BinderBase[] allBinders)
        {
            var allDependedBinders = allBinders.OfType<ObservableBinder>()
                .Where(b => b.BindingType == BindingType.Binder && ReferenceEquals(b.SourceBinder, sourceBinder));
            foreach (var dependedBinder in allDependedBinders)
            {
                _errorObjects.Add(dependedBinder.gameObject.GetInstanceID());
                dependedBinder.SmartReset();
                Debug.Log($"{dependedBinder.gameObject.name} marked as error as depended Binder", dependedBinder.gameObject);
            }
        }

        private static void UpdateAllDependedBinders(View view, BinderBase[] allBinders)
        {
            var allDependencies = allBinders.Where(b => ReferenceEquals(b.SourceView, view));

            foreach (var dependedBinder in allDependencies)
            {
                if (dependedBinder.IsBroken())
                {
                    MarkSelf(dependedBinder);
                    MarkParents(dependedBinder);
                    UpdateAllDependedBinders(dependedBinder, allBinders);
                }
            }

        }

        private static void UpdateAllDependedBinders(BinderBase targetBinder, BinderBase[] allBinders)
        {
            var allDependencies = allBinders.Where(b =>
            {
                if (b is ObservableBinder ob)
                {
                    return ReferenceEquals(ob.SourceBinder, targetBinder);
                }

                return false;
            });

            foreach (var dependedBinder in allDependencies)
            {
            }

        }
        
        
        private static bool ValidateView(View view)
        {
            if (string.IsNullOrEmpty(view.ViewModelTypeFullName))
            {
                view.ResetPropertyName();
                _errorObjects.Add(view.gameObject.GetInstanceID());
                return false;
            }

            return true;
        }

        private static void ValidateBinders()
        {
            
        }
        
    }
}