using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    [CustomEditor(typeof(View))]
    public class ViewEditor : UnityEditor.Editor
    {
        private SerializedProperty _viewModelTypeFullName;
        private SerializedProperty _viewModelPropertyName;
        private View _view;        
        private SerializedProperty _parentView;
        private StringListSearchProvider _searchProvider;
        
        // help 
        private readonly List<View> _parentViews = new();
        private readonly List<string> _viewModelPropertyNames = new();

        private void OnEnable()
        {
            _searchProvider = CreateInstance<StringListSearchProvider>();
            _view = (View)target;
            _viewModelTypeFullName = serializedObject.FindProperty(nameof(_viewModelTypeFullName));
            _viewModelPropertyName = serializedObject.FindProperty(nameof(_viewModelPropertyName));
            _parentView = serializedObject.FindProperty(nameof(_parentView));
        }

        public override void OnInspectorGUI()
        {
            DrawScriptTitle();
            DrawParentViewField();

            var isParentViewExist = _parentView.objectReferenceValue != null;

            if (isParentViewExist)
            {
                DrawViewModelPropertyNameSelector();
                DrawViewModelLabel();
                DrawSelectParentViewButton();
            }
            else
            {
                DrawViewModelSelector();
            }
            
            DrawOpenViewModelButton();

            serializedObject.ApplyModifiedProperties();
        }
        
        protected void DrawParentViewField()
        {
            var oldParentViewValue = _parentView.objectReferenceValue as View;
            EditorGUILayout.PropertyField(_parentView);
            serializedObject.ApplyModifiedProperties();

            var newParentView = _parentView.objectReferenceValue as View;
            if (!ReferenceEquals(newParentView, oldParentViewValue))
            {
                _viewModelTypeFullName.stringValue = null;
                _viewModelPropertyName.stringValue = null;
                serializedObject.ApplyModifiedProperties();
                _view.CheckValidation();
            }
        }
        
        private void DrawScriptTitle()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField(MVVMConstants.SCRIPT, MonoScript.FromMonoBehaviour((View)target), typeof(View), false);
            GUI.enabled = true;
        }
        
        private void DrawViewModelSelector()
        {
            var viewModelTypeFullNames = ViewModelsDB.AllViewModelTypeFullNames;

            _searchProvider.Init(viewModelTypeFullNames, newViewModelTypeFullNameSelected =>
            {
                _viewModelTypeFullName.stringValue =
                    newViewModelTypeFullNameSelected == MVVMConstants.NONE ? null : newViewModelTypeFullNameSelected;
                serializedObject.ApplyModifiedProperties();

                _view.CheckValidation();
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

        private void DrawViewModelLabel()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(MVVMConstants.VIEW_MODEL);
            
            var viewModelTypeFullName = _viewModelTypeFullName.stringValue;
            var isViewModelSelected = !string.IsNullOrEmpty(viewModelTypeFullName);
            var viewModelType = ViewModelsEditorUtility.ConvertViewModelType(viewModelTypeFullName);
            var displayedViewModelName = isViewModelSelected ? viewModelType.Name : MVVMConstants.NONE;
            EditorGUILayout.LabelField(displayedViewModelName);

            EditorGUILayout.EndHorizontal();
        }

        private void DrawViewModelPropertyNameSelector()
        {
            var parentView = _parentView.objectReferenceValue as View;
            var parentViewModelPropertyNames = GetAllViewModelPropertyNames(parentView);
            var selectedViewModelPropertyName = _viewModelPropertyName.stringValue;
            var isPropertySelected = !string.IsNullOrEmpty(selectedViewModelPropertyName);
            if (isPropertySelected && !parentViewModelPropertyNames.Contains(selectedViewModelPropertyName))
            {
                Debug.LogWarning("Parent view model was changed, connected property name has been reset to None",
                    _view.gameObject);
                _viewModelPropertyName.stringValue = null;
                serializedObject.ApplyModifiedProperties();
            }

            DrawSearchPanelForViewModelProperties(parentView.ViewModelTypeFullName,
                parentViewModelPropertyNames);
        }

        private void DrawSelectParentViewButton()
        {
            var parentView = _parentView.objectReferenceValue as MonoBehaviour;
            if (parentView != null)
            {
                if (GUILayout.Button($"Select Parent View"))
                {
                    Selection.activeGameObject = parentView.gameObject;
                }
            }
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
            
            serializedObject.ApplyModifiedProperties();
            
            _view.CheckValidation();
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