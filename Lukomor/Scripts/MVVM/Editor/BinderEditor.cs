using System;
using System.Collections.Generic;
using System.Linq;
using Lukomor.MVVM.Binders;
using UnityEditor;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    [CustomEditor(typeof(BinderDEPRECATED), true)]
    public abstract class BinderEditor : UnityEditor.Editor
    {
        private BinderDEPRECATED _binderDeprecated;
        private View _parentView;
        private SerializedProperty _viewModelTypeFullName;
        private SerializedProperty _binderId;

        protected SerializedProperty ViewModelTypeFullName => _viewModelTypeFullName;
        private TypeCache.TypeCollection _cachedViewModelTypes;

        protected virtual void OnEnable()
        {
            _binderDeprecated = (BinderDEPRECATED)target;
            _parentView = _binderDeprecated.GetComponentInParent<View>(true);

            _viewModelTypeFullName = serializedObject.FindProperty(nameof(_viewModelTypeFullName));
            _binderId = serializedObject.FindProperty("_id");
        }

        public sealed override void OnInspectorGUI()
        {
            ShowInstanceId();

            base.OnInspectorGUI();

            var allParentViews = _binderDeprecated.GetComponentsInParent<View>(true);
            var allParentViewModelTypeFullNamesMap = new Dictionary<string, View>();
            foreach (var parentView in allParentViews)
            {
                if (string.IsNullOrEmpty(parentView.ViewModelTypeFullName))
                {
                    continue;
                }

                allParentViewModelTypeFullNamesMap[parentView.ViewModelTypeFullName] = parentView;
            }
            var allParentBinders = _binderDeprecated.GetComponentsInParent<BinderDEPRECATED>().Where(b => !ReferenceEquals(b, _binderDeprecated))
                .ToArray();

            _cachedViewModelTypes = TypeCache.GetTypesDerivedFrom<IViewModel>();

            _viewModelTypeFullName.stringValue = _parentView.ViewModelTypeFullName;

            if (string.IsNullOrWhiteSpace(_viewModelTypeFullName.stringValue))
            {
                EditorGUILayout.HelpBox("There is no view model setup on the View. Please check View setup.",
                    MessageType.Warning);
                return;
            }

            DrawProperties();
            DrawViewModelDebug(_viewModelTypeFullName.stringValue);
        }

        private void ShowInstanceId()
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(_binderId);
            GUI.enabled = true;
        }

        private void DrawViewModelDebug(string viewModelFullName)
        {
            var viewModelType = GetViewModelType(viewModelFullName);

            if (viewModelType != null)
            {
                GUI.enabled = false;
                EditorGUILayout.LabelField("ViewModel: " + viewModelType.Name);
                GUI.enabled = true;
            }
        }

        protected abstract void DrawProperties();
        
        protected static bool IsValidProperty(Type propertyType, Type requiredType, Type requiredArgumentType)
        {
            var genericArgument = propertyType.GetGenericArguments().First();

            if (!requiredArgumentType.IsAssignableFrom(genericArgument))
            {
                return false;
            }
            
            var interfaceTypes = propertyType.GetInterfaces().Where(i => i.IsGenericType);

            if (requiredType.IsAssignableFrom(propertyType.GetGenericTypeDefinition()))
            {
                return true;
            }

            foreach (var interfaceType in interfaceTypes)
            {
                if (requiredType.IsAssignableFrom(interfaceType.GetGenericTypeDefinition()))
                {
                    return true;
                }
            }

            return false;
        }
        
        protected static bool IsValidPropertyName(string propertyName, Type viewModelType)
        {
            var property = viewModelType.GetProperty(propertyName);

            return property != null;
        }

        protected static bool IsValidMethodName(string methodName, Type viewModelType)
        {
            var method = viewModelType.GetMethod(methodName);

            return method != null;
        }

        protected static Type GetViewModelType(string viewModelFullName)
        {
            var allViewModelTypes = ViewModelsDB.AllViewModelTypes;
            var viewModelType = allViewModelTypes.FirstOrDefault(t => t.FullName == viewModelFullName);

            return viewModelType;
        }
    }
}