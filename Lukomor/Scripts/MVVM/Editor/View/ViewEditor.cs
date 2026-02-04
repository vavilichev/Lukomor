#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    [CustomEditor(typeof(View))]
    public class ViewEditor : UnityEditor.Editor
    {
        private readonly List<string> _viewModelPropertyNames = new();
        private SerializedProperty _viewModelTypeFullName;
        private SerializedProperty _viewModelPropertyName;
        private View _view;
        private SerializedProperty _sourceView;
        private StringListSearchProvider _searchProvider;

        private void OnEnable()
        {
            _searchProvider = CreateInstance<StringListSearchProvider>();
            _view = (View)target;
            _viewModelTypeFullName = serializedObject.FindProperty(nameof(_viewModelTypeFullName));
            _viewModelPropertyName = serializedObject.FindProperty(nameof(_viewModelPropertyName));
            _sourceView = serializedObject.FindProperty(nameof(_sourceView));
        }

        public override void OnInspectorGUI()
        {
            MVVMEditorLayout.DrawScriptTitle(_view);
            DrawSourceViewField();
            ViewModelsEditorUtility.ValidateViewModel(serializedObject, _viewModelTypeFullName);

            var isSourceViewExist = _sourceView.objectReferenceValue != null;

            if (isSourceViewExist)
            {
                DrawViewModelPropertyNameSelector();
                DrawViewModelLabel();
                DrawSelectSourceViewButton();
            }
            else
            {
                DrawViewModelSelector();
            }

            DrawOpenViewModelButton();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSourceViewField()
        {
            var oldSourceViewValue = _sourceView.objectReferenceValue as View;
            EditorGUILayout.PropertyField(_sourceView);
            serializedObject.ApplyModifiedProperties();

            var newSourceView = _sourceView.objectReferenceValue as View;
            if (!ReferenceEquals(newSourceView, oldSourceViewValue))
            {
                _viewModelTypeFullName.stringValue = null;
                _viewModelPropertyName.stringValue = null;
                serializedObject.ApplyModifiedProperties();
                MVVMValidator.RequestValidation();
            }
        }
        
        private void DrawViewModelPropertyNameSelector()
        {
            var sourceView = _sourceView.objectReferenceValue as View;
            var sourceViewModelPropertyNames = GetAllViewModelPropertyNames(sourceView);
            var selectedViewModelPropertyName = _viewModelPropertyName.stringValue;
            var isPropertySelected = !string.IsNullOrEmpty(selectedViewModelPropertyName);
            if (isPropertySelected && !sourceViewModelPropertyNames.Contains(selectedViewModelPropertyName))
            {
                Debug.LogWarning("Source view model was changed, connected property name has been reset to None",
                                 _view.gameObject);
                _viewModelPropertyName.stringValue = null;
                serializedObject.ApplyModifiedProperties();
            }

            DrawSearchPanelForViewModelProperties(sourceView.ViewModelTypeFullName,
                                                  sourceViewModelPropertyNames);
        }

        private ICollection<string> GetAllViewModelPropertyNames(View sourceView)
        {
            _viewModelPropertyNames.Clear();

            var sourceViewModelTypeFullName = sourceView.ViewModelTypeFullName;
            var sourceViewModelType = ViewModelsEditorUtility.ConvertViewModelType(sourceViewModelTypeFullName);

            if (sourceViewModelType == null)
            {
                var logLine = $"ViewModel for Source View didn't selected, " +
                              $"the list of properties cannot be defined. " +
                              $"Please, check the View Model setup for Source View ({sourceView.name})";

                EditorGUILayout.HelpBox(logLine, MessageType.Warning);

                return _viewModelPropertyNames; // source view model is not selected
            }

            var allViewModelPropertyNames =
                ViewModelsEditorUtility.GetAllValidViewModelPropertyNames(sourceViewModelType);
            _viewModelPropertyNames.AddRange(allViewModelPropertyNames);

            return _viewModelPropertyNames;
        }

        private void DrawSearchPanelForViewModelProperties(string sourceViewModelTypeFullName,
                                                           ICollection<string> propertyNames)
        {
            var viewModelPropertiesWithSubViewModelNames = propertyNames.ToArray();

            _searchProvider.Init(viewModelPropertiesWithSubViewModelNames,
                                 selectedViewModelPropertyName =>
                                 {
                                     SelectNewViewModelBySourceProperty(selectedViewModelPropertyName,
                                                                        sourceViewModelTypeFullName);
                                 });

            EditorGUILayout.BeginHorizontal();

            var selectedViewModelType =
                GetViewModelTypeByPropertyName(sourceViewModelTypeFullName, _viewModelPropertyName.stringValue);
            var propertyTypeName = selectedViewModelType != null ? $" ({selectedViewModelType.Name})" : string.Empty;

            EditorGUILayout.LabelField($"Property Name{propertyTypeName}:");

            var displayName = string.IsNullOrEmpty(_viewModelPropertyName.stringValue)
                ? MVVMConstants.NONE
                : _viewModelPropertyName.stringValue;

            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                                  _searchProvider);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void SelectNewViewModelBySourceProperty(string selectedViewModelPropertyName,
                                                        string sourceViewModelTypeFullName)
        {
            if (selectedViewModelPropertyName != MVVMConstants.NONE)
            {
                var selectedViewModelType =
                    GetViewModelTypeByPropertyName(sourceViewModelTypeFullName, selectedViewModelPropertyName);

                _viewModelPropertyName.stringValue = selectedViewModelPropertyName;
                _viewModelTypeFullName.stringValue = selectedViewModelType.FullName;
            }
            else
            {
                _viewModelPropertyName.stringValue = null;
                _viewModelTypeFullName.stringValue = null;
            }

            serializedObject.ApplyModifiedProperties();

            MVVMValidator.RequestValidation();
        }

        private Type GetViewModelTypeByPropertyName(string sourceViewModelTypeFullName, string viewModelPropertyName)
        {
            if (string.IsNullOrEmpty(viewModelPropertyName))
            {
                return null;
            }

            var sourceViewModelType = ViewModelsEditorUtility.ConvertViewModelType(sourceViewModelTypeFullName);
            var allSourceViewModelProperties = sourceViewModelType.GetProperties();
            var selectedProperty =
                allSourceViewModelProperties.First(p => p.Name == viewModelPropertyName);
            var viewModelType = selectedProperty.PropertyType.GetGenericArguments().First();

            return viewModelType;
        }

        private void DrawViewModelLabel()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(MVVMConstants.VIEW_MODEL);

            var viewModelTypeFullName = _viewModelTypeFullName.stringValue;
            var isViewModelSelected = !string.IsNullOrEmpty(viewModelTypeFullName);
            var viewModelType = ViewModelsEditorUtility.ConvertViewModelType(viewModelTypeFullName);
            var isViewModelValid = viewModelType != null;
            if (!isViewModelValid)
            {
                _viewModelTypeFullName.stringValue = null;
                _viewModelPropertyName.stringValue = null;
                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndHorizontal();
                return;
            }
            
            var displayedViewModelName = isViewModelSelected ? viewModelType.Name : MVVMConstants.NONE;
            EditorGUILayout.LabelField(displayedViewModelName);

            EditorGUILayout.EndHorizontal();

            if (!isViewModelSelected)
            {
                EditorGUILayout.HelpBox("Please, select view model property from the source view", MessageType.Warning);
            }
        }

        private void DrawSelectSourceViewButton()
        {
            var sourceView = _sourceView.objectReferenceValue as MonoBehaviour;
            if (sourceView != null)
            {
                if (GUILayout.Button($"Select Source View"))
                {
                    Selection.activeGameObject = sourceView.gameObject;
                }
            }
        }

        private void DrawViewModelSelector()
        {
            var viewModelTypeFullNames = ViewModelsDB.AllViewModelTypeFullNames;

            _searchProvider.Init(viewModelTypeFullNames, newViewModelTypeFullNameSelected =>
            {
                _viewModelTypeFullName.stringValue =
                    newViewModelTypeFullNameSelected == MVVMConstants.NONE ? null : newViewModelTypeFullNameSelected;
                serializedObject.ApplyModifiedProperties();

                MVVMValidator.RequestValidation();
            });

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(MVVMConstants.VIEW_MODEL);

            var displayName = string.IsNullOrEmpty(_viewModelTypeFullName.stringValue)
                ? MVVMConstants.NONE
                : ViewModelsEditorUtility.ToShortName(_viewModelTypeFullName.stringValue);

            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                var mousePos = Event.current.mousePosition;
                mousePos.Set(mousePos.x - 200, mousePos.y);
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(mousePos), 400), _searchProvider);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawOpenViewModelButton()
        {
            var viewModelTypeFullName = _viewModelTypeFullName.stringValue;
            if (!string.IsNullOrEmpty(viewModelTypeFullName))
            {
                var viewModelType = ViewModelsEditorUtility.ConvertViewModelType(viewModelTypeFullName);
                var viewModelTypeDisplayedName = viewModelType.Name;

                if (GUILayout.Button($"Open {viewModelTypeDisplayedName}"))
                {
                    OpenScript(viewModelType.Name);
                }
            }
        }

        private void OpenScript(string typeName)
        {
            var guids = AssetDatabase.FindAssets($"t: script {typeName}");

            if (guids.Length > 0)
            {
                var scriptPath = AssetDatabase.GUIDToAssetPath(guids[0]);

                EditorUtility.OpenWithDefaultApp(scriptPath);
            }
            else
            {
                Debug.LogError($"No script found for type: {typeName}");
            }
        }
    }
}
#endif