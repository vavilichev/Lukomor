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
        private const string NONE = "None";
        private SerializedProperty _viewModelTypeFullName;
        private SerializedProperty _viewModelPropertyName;
        private SerializedProperty _isParentView;
        private SerializedProperty _childBinders;
        private SerializedProperty _subViews;
        private View _view;
        private readonly Dictionary<string, string> _viewModelNames = new();
        private readonly List<string> _viewModelPropertyNames = new();

        private void OnEnable()
        {
            _view = (View)target;
            _viewModelTypeFullName = serializedObject.FindProperty(nameof(_viewModelTypeFullName));
            _viewModelPropertyName = serializedObject.FindProperty(nameof(_viewModelPropertyName));
            _isParentView = serializedObject.FindProperty(nameof(_isParentView));
            _childBinders = serializedObject.FindProperty(nameof(_childBinders));
            _subViews = serializedObject.FindProperty(nameof(_subViews));
        }

        public override void OnInspectorGUI()
        {
            DrawScriptTitle();
            
            var parentView = _view.GetComponentsInParent<View>().FirstOrDefault(c => !ReferenceEquals(c, _view));
            var provider = CreateInstance<StringListSearchProvider>();
            var isParentViewExist = parentView != null;
            var parentViewGo = isParentViewExist ? parentView.gameObject : _view.gameObject;

            if (isParentViewExist && !string.IsNullOrEmpty(parentView.ViewModelTypeFullName))
            {
                SetParentViewBoolean(false);
                DefineAllViewModelPropertyNames(parentView.ViewModelTypeFullName);
                DrawEditorForSubView(provider, parentView.ViewModelTypeFullName);
                DrawDebug();

                var childViewModelType = GetChildViewModelType(parentView.ViewModelTypeFullName, _view.ViewModelPropertyName);
                
                DrawSubViewModelDebugButtons(parentViewGo, childViewModelType?.FullName);
            }
            else
            {
                SetParentViewBoolean(true);
                DefineAllViewModels();
                DrawEditorForParentView(provider);
                DrawDebug();
                DrawOpenViewModelButton(_view.ViewModelTypeFullName);
            }
        }

        private void DrawEditorForSubView(StringListSearchProvider provider, string parentViewModelTypeFullName)
        {
            var options = _viewModelPropertyNames.ToArray();

            provider.Init(options, result =>
            {
                _viewModelPropertyName.stringValue = result == NONE ? null : result;

                if (result != NONE)
                {
                    _viewModelTypeFullName.stringValue = GetChildViewModelType(parentViewModelTypeFullName, result)?.FullName;
                }

                serializedObject.ApplyModifiedProperties();
            });
                
            EditorGUILayout.BeginHorizontal();

            var childPropertyType = GetChildViewModelType(parentViewModelTypeFullName, _viewModelPropertyName.stringValue);
            var propertyTypeName = childPropertyType != null ? $" ({childPropertyType.Name})" : string.Empty;
            
            EditorGUILayout.LabelField($"Property Name{propertyTypeName}:");

            var displayName = string.IsNullOrEmpty(_viewModelPropertyName.stringValue)
                ? NONE
                : _viewModelPropertyName.stringValue;
            
            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), provider);
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private void DrawEditorForParentView(StringListSearchProvider provider)
        {
            var options = _viewModelNames.Keys.ToArray();

            provider.Init(options, result =>
            {
                _viewModelTypeFullName.stringValue = _viewModelNames[result];

                serializedObject.ApplyModifiedProperties();
            });
                
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("ViewModel:");

            var displayName = string.IsNullOrEmpty(_viewModelTypeFullName.stringValue)
                ? NONE
                : ToShortName(_viewModelTypeFullName.stringValue);
            
            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), provider);
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void SetParentViewBoolean(bool isParentView)
        {
            if (Application.isPlaying)
            {
                return;
            }
            
            _isParentView.boolValue = isParentView;

            serializedObject.ApplyModifiedProperties();
        }

        private void DefineAllViewModels()
        {
            _viewModelNames.Clear();
            _viewModelNames[NONE] = null;

            var allViewModelsTypes = TypeCache.GetTypesDerivedFrom<IViewModel>()
                .Where(myType => myType.IsClass && !myType.IsAbstract);
            
            foreach (var viewModelsType in allViewModelsTypes)
            {
                _viewModelNames[viewModelsType.Name] = viewModelsType.FullName;
            }
        }

        private void DefineAllViewModelPropertyNames(string parentViewModelTypeFullName)
        {
            _viewModelPropertyNames.Clear();
            _viewModelPropertyNames.Add(NONE);

            var parentViewModelType = Type.GetType(parentViewModelTypeFullName);

            var allViewModelProperties = parentViewModelType.GetProperties();
            var allValidProperties =
                allViewModelProperties.Where(p => typeof(IViewModel).IsAssignableFrom(p.PropertyType));

            foreach (var validProperty in allValidProperties)
            {
                _viewModelPropertyNames.Add(validProperty.Name);
            }
        }

        private string ToShortName(string viewModelTypeFullName)
        {
            var viewModelType = Type.GetType(viewModelTypeFullName);
            
            return viewModelType == null ? NONE : viewModelType.Name;
        }

        private void DrawScriptTitle()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((View)target), typeof(View), false);
            GUI.enabled = true;
        }

        private void DrawDebug()
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(_isParentView);
            EditorGUILayout.PropertyField(_subViews);
            EditorGUILayout.PropertyField(_childBinders);
            GUI.enabled = true;
        }

        private void DrawPingParentViewButton(GameObject parentViewGo)
        {
            if (parentViewGo != null && GUILayout.Button("Highlight Parent View"))
            {
                EditorGUIUtility.PingObject(parentViewGo);
            }
        }

        private void DrawOpenViewModelButton(string viewModelTypeFullName)
        {
            if (!string.IsNullOrEmpty(viewModelTypeFullName))
            {
                var viewModelType = Type.GetType(viewModelTypeFullName);

                if (GUILayout.Button($"Open {viewModelType.Name}"))
                {
                    OpenScript(viewModelType.Name);
                }
            }
        }

        private void DrawSubViewModelDebugButtons(GameObject parentViewGo, string viewModelTypeFullName)
        {
            EditorGUILayout.BeginHorizontal();

            DrawPingParentViewButton(parentViewGo);
            DrawOpenViewModelButton(viewModelTypeFullName);
            
            EditorGUILayout.EndHorizontal();
        }
        
        private Type GetChildViewModelType(string parentViewModelTypeFullName, string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                var parentViewModelType = Type.GetType(parentViewModelTypeFullName);
                var property = parentViewModelType.GetProperty(propertyName);
  
                return property.PropertyType;
            }
            
            return null;
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