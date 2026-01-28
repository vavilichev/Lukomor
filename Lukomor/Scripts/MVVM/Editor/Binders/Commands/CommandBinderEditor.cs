using System.Collections.Generic;
using System.Linq;
using Lukomor.MVVM.Binders;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor.Binders
{
    [CustomEditor(typeof(CommandBinderBase), true)]
    public class CommandBinderBaseEditor : UnityEditor.Editor
    {
        private const string PROP_SOURCE_VIEW = "_sourceView";
        private const string PROP_VIEW_MODEL_PROPERTY_NAME = "_viewModelCommandPropertyName";
        
        private static readonly HashSet<string> SetupFields = new()
        {
            PROP_SOURCE_VIEW,
            PROP_VIEW_MODEL_PROPERTY_NAME,
        };
        
        private CommandBinderBase _binder;
        private SerializedProperty _sourceViewProperty;
        private SerializedProperty _viewModelCommandPropertyNameProperty;
        private StringListSearchProvider _searchProvider;

        protected void OnEnable()
        {
            _searchProvider = CreateInstance<StringListSearchProvider>();
            _binder = (CommandBinderBase)target;
            _sourceViewProperty = serializedObject.FindProperty(PROP_SOURCE_VIEW);
            _viewModelCommandPropertyNameProperty = serializedObject.FindProperty(PROP_VIEW_MODEL_PROPERTY_NAME);
        }

        public sealed override void OnInspectorGUI()
        {
            DrawCustomHeader();
            DrawInheritedProperties();
            CheckValidation();

            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawCustomHeader()
        {
            MVVMEditorUtils.DrawScriptTitle(_binder);
            DrawSourceViewProperty();
            DrawSourceViewPropertyNameProperty();
        }
        
        private void DrawSourceViewProperty()
        {
            EditorGUILayout.PropertyField(_sourceViewProperty);
            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawSourceViewPropertyNameProperty()
        {
            if (_sourceViewProperty.objectReferenceValue == null)
            {
                return;
            }

            var sourceView = _sourceViewProperty.objectReferenceValue as View;
            var sourceViewModelTypeFullName = sourceView?.ViewModelTypeFullName;
            if (string.IsNullOrEmpty(sourceViewModelTypeFullName))
            {
                // view model type must be selected
                return;
            }

            DrawSelectViewModelPropertyLine(sourceViewModelTypeFullName);
        }
        
        private void DrawSelectViewModelPropertyLine(string sourceViewModelTypeFullName)
        {
            EditorGUILayout.BeginHorizontal();

            var sourceViewModelType = ViewModelsEditorUtility.ConvertViewModelType(sourceViewModelTypeFullName);
            var labelText = $"{MVVMConstants.PROPERTY_NAME} ({sourceViewModelType.Name}):";
            EditorGUILayout.LabelField(labelText);

            var displayName = string.IsNullOrEmpty(_viewModelCommandPropertyNameProperty.stringValue)
                ? MVVMConstants.NONE
                : _viewModelCommandPropertyNameProperty.stringValue;
            
            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                var sourceViewModelProperties = sourceViewModelType.GetProperties();
                var commandType = _binder.CommandType;
                var sourceViewModelValidProperties =
                    ViewModelsEditorUtility.FilterValidProperties(sourceViewModelProperties, commandType);
                var sourceViewModelValidPropertyNames = sourceViewModelValidProperties.Select(x => x.Name).ToArray();
                
                _searchProvider.Init(sourceViewModelValidPropertyNames, newPropertyNameSelected =>
                {
                    _viewModelCommandPropertyNameProperty.stringValue =
                        newPropertyNameSelected == MVVMConstants.NONE ? null : newPropertyNameSelected;
                    serializedObject.ApplyModifiedProperties();
                });
                
                var mousePos = Event.current.mousePosition;
                mousePos.Set(mousePos.x, mousePos.y);
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(mousePos), 250), _searchProvider);
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawInheritedProperties()
        {
            var iterator = serializedObject.GetIterator();
            EditorGUILayout.Space();
            
            var enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;

                if (iterator.name == "m_Script")
                    continue;

                if (SetupFields.Contains(iterator.name))
                    continue;

                EditorGUILayout.PropertyField(iterator, true);
            }
        }

        private void CheckValidation()
        {
            var sourceView = _sourceViewProperty.objectReferenceValue as View;
            if (sourceView == null)
            {
                EditorGUILayout.HelpBox("No View selected", MessageType.Warning);
                return;
            }

            var commandName = _viewModelCommandPropertyNameProperty.stringValue;
            if (string.IsNullOrEmpty(commandName))
            {
                EditorGUILayout.HelpBox("View Model Command wasn't selected", MessageType.Warning);
                return;
            }
        }
    }
}