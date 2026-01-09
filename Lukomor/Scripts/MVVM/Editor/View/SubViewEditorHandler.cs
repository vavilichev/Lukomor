using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    public class SubViewEditorHandler : ViewEditorHandler
    {
        private readonly SerializedObject _serializedObject;
        private SerializedProperty _parentView;
        /// <summary>
        /// Sekected View Model Property Name (as it's sub view)
        /// </summary>
        private readonly SerializedProperty _viewModelPropertyName;
        /// <summary>
        /// Selected (by property) View Model Type Full Name
        /// </summary>
        private readonly SerializedProperty _viewModelTypeFullName;
        private readonly StringListSearchProvider _searchProvider;
        private readonly View _view;
        private readonly List<string> _viewModelPropertyNames = new();

        public SubViewEditorHandler(SerializedObject serializedObject, StringListSearchProvider searchProvider, View view) : base(
            serializedObject, view)
        {
            _serializedObject = serializedObject;
            _parentView = serializedObject.FindProperty(nameof(_parentView));
            _viewModelTypeFullName = serializedObject.FindProperty(nameof(_viewModelTypeFullName));
            _viewModelPropertyName = serializedObject.FindProperty(nameof(_viewModelPropertyName));
            _searchProvider = searchProvider;
            _view = view;
        }

        public void DrawEditor()
        {
            DrawParentViewField();
            
            // parent MUST exist
            var parentView = _parentView.objectReferenceValue as View;
            var isParentViewExist = parentView != null;
            if (!isParentViewExist)
            {
                throw new Exception("Parent view not found. Remember that Parent view must be higher in the hierarchy to work properly.");
            }

            // If parent view model is empty, let's show a message about it
            var parentViewModelTypeFullName = parentView.ViewModelTypeFullName;
            if (string.IsNullOrEmpty(parentViewModelTypeFullName))
            {
                EditorGUILayout.HelpBox(
                    $"Parent View Model is not defined. Please, define View Model in the parent View: ({parentView.name})",
                    MessageType.Warning);
                
                if (GUILayout.Button($"Select Parent View"))
                {
                    Selection.activeGameObject = parentView.gameObject;
                }
                
                return; // Parent View Model isn't selected.
            }

            var parentViewModelPropertyNames = GetAllViewModelPropertyNames(parentView);
            var selectedViewModelPropertyName = _viewModelPropertyName.stringValue;
            var isPropertySelected = !string.IsNullOrEmpty(selectedViewModelPropertyName);
            if (isPropertySelected && !parentViewModelPropertyNames.Contains(selectedViewModelPropertyName))
            {
                Debug.LogWarning("Parent view model was changed, connected property name has been reset to None",
                    _view.gameObject);
                _viewModelPropertyName.stringValue = null;
                _serializedObject.ApplyModifiedProperties();
            }

            DrawSearchPanelForViewModelProperties(parentView.ViewModelTypeFullName,
                parentViewModelPropertyNames);

            DrawDebug();
        }

        private ICollection<string> GetAllViewModelPropertyNames(View parentView)
        {
            _viewModelPropertyNames.Clear();

            var parentViewModelTypeFullName = parentView.ViewModelTypeFullName;
            var parentViewModelType = ViewModelsEditorUtility.ConvertViewModelType(parentViewModelTypeFullName);

            if (parentViewModelType == null)
            {
                var logLine = $"ViewModel for Parent View didn't selected, " +
                              $"the list of properties cannot be defined. " +
                              $"Please, check the View Model setup for parent View ({parentView.name})";
                
                EditorGUILayout.HelpBox(logLine, MessageType.Warning);
                Debug.LogWarning(logLine, parentView.gameObject);

                return _viewModelPropertyNames; // parent view model is not selected
            }

            var allViewModelPropertyNames = ViewModelsEditorUtility.GetAllValidViewModelPropertyNames(parentViewModelType);
            _viewModelPropertyNames.AddRange(allViewModelPropertyNames);

            return _viewModelPropertyNames;
        }
        
        private void DrawSearchPanelForViewModelProperties(string parentViewModelTypeFullName, ICollection<string> propertyNames)
        {
            var viewModelPropertiesWithSubViewModelNames = propertyNames.ToArray();

            _searchProvider.Init(viewModelPropertiesWithSubViewModelNames, selectedViewModelPropertyName =>
            {
                SelectNewViewModelByParentProperty(selectedViewModelPropertyName, parentViewModelTypeFullName);
            });
                
            EditorGUILayout.BeginHorizontal();

            var selectedViewModelType = GetViewModelTypeByPropertyName(parentViewModelTypeFullName, _viewModelPropertyName.stringValue);
            var propertyTypeName = selectedViewModelType != null ? $" ({selectedViewModelType.Name})" : string.Empty;
            
            EditorGUILayout.LabelField($"Property Name{propertyTypeName}:");

            var displayName = string.IsNullOrEmpty(_viewModelPropertyName.stringValue)
                ? MVVMConstants.NONE
                : _viewModelPropertyName.stringValue;
            
            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), _searchProvider);
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private void SelectNewViewModelByParentProperty(string selectedViewModelPropertyName, string parentViewModelTypeFullName)
        {
            if (selectedViewModelPropertyName != MVVMConstants.NONE)
            {
                var selectedViewModelType =
                    GetViewModelTypeByPropertyName(parentViewModelTypeFullName, selectedViewModelPropertyName);
                    
                _viewModelPropertyName.stringValue = selectedViewModelPropertyName;
                _viewModelTypeFullName.stringValue = selectedViewModelType.FullName;
            }
            else
            {
                _viewModelPropertyName.stringValue = null;
                _viewModelTypeFullName.stringValue = null;
            }
            
            _serializedObject.ApplyModifiedProperties();
            
            _view.CheckForWarningIcon();
            CheckSubViews();
        }

        private Type GetViewModelTypeByPropertyName(string parentViewModelTypeFullName, string viewModelPropertyName)
        {
            if (string.IsNullOrEmpty(viewModelPropertyName))
            {
                return null;
            }
            
            var parentViewModelType = ViewModelsEditorUtility.ConvertViewModelType(parentViewModelTypeFullName);
            var allParentViewModelProperties = parentViewModelType.GetProperties();
            var selectedProperty =
                allParentViewModelProperties.First(p => p.Name == viewModelPropertyName);
            var viewModelType = selectedProperty.PropertyType.GetGenericArguments().First();
            
            return viewModelType;
        }
    }
}