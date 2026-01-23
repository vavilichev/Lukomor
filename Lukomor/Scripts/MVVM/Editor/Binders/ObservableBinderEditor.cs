using System;
using System.Collections.Generic;
using System.Linq;
using Lukomor.MVVM.Binders;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor.Binders
{
    [CustomEditor(typeof(ObservableBinder), true)]
    public class ObservableBinderEditor : UnityEditor.Editor
    {
        private const string PROP_BINDING_TYPE = "_bindingType";
        private const string PROP_SOURCE_VIEW = "_sourceView";
        private const string PROP_VIEW_MODEL_PROPERTY_NAME = "_viewModelPropertyName";
        private const string PROP_SOURCE_BINDER = "_sourceBinder";
        
        private static readonly HashSet<string> SetupFields = new()
        {
            PROP_BINDING_TYPE,
            PROP_SOURCE_VIEW,
            PROP_VIEW_MODEL_PROPERTY_NAME,
            PROP_SOURCE_BINDER
        };
        
        private StringListSearchProvider _searchProvider;
        private ObservableBinder _binder;
        private SerializedProperty _bindingTypeProperty;
        private SerializedProperty _sourceViewProperty;
        private SerializedProperty _viewModelPropertyNameProperty;
        private SerializedProperty _sourceBinderProperty;

        protected void OnEnable()
        {
            _searchProvider = CreateInstance<StringListSearchProvider>();
            _binder = (ObservableBinder)target;
            _bindingTypeProperty = serializedObject.FindProperty(PROP_BINDING_TYPE);
            _sourceViewProperty = serializedObject.FindProperty(PROP_SOURCE_VIEW);
            _viewModelPropertyNameProperty = serializedObject.FindProperty(PROP_VIEW_MODEL_PROPERTY_NAME);
            _sourceBinderProperty = serializedObject.FindProperty(PROP_SOURCE_BINDER);
        }

        public sealed override void OnInspectorGUI()
        {
            DrawCustomHeader();
            DrawInheritedProperties();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCustomHeader()
        {
            DrawScriptTitle();
            DrawBindingTypeProperty();
            
            if (_binder.BindingType == BindingType.View)
            {
                DrawSourceViewSetup();
            }
            else
            {
                DrawSourceBinderProperty();
            }
        }

        private void DrawScriptTitle()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField(MVVMConstants.SCRIPT, MonoScript.FromMonoBehaviour(_binder), typeof(ObservableBinder), false);
            GUI.enabled = true;
        }
        
        private void DrawBindingTypeProperty()
        {
            var oldBingingType = (BindingType)_bindingTypeProperty.enumValueIndex;
            EditorGUILayout.PropertyField(_bindingTypeProperty);
            var newBindingType = (BindingType)_bindingTypeProperty.enumValueIndex;
            
            if (oldBingingType == newBindingType)
            {
                return;
            }

            if (newBindingType == BindingType.View)
            {
                _sourceBinderProperty.objectReferenceValue = null;
            }
            else
            {
                _sourceViewProperty.objectReferenceValue = null;
                _viewModelPropertyNameProperty.stringValue = null;
            }
        }
        
        private void DrawSourceViewSetup()
        {
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

            var displayName = string.IsNullOrEmpty(_viewModelPropertyNameProperty.stringValue)
                ? MVVMConstants.NONE
                : _viewModelPropertyNameProperty.stringValue;
            
            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                var sourceViewModelProperties = sourceViewModelType.GetProperties();
                var binderInputValueType = _binder.InputType;
                var sourceViewModelValidProperties =
                    ViewModelsEditorUtility.FilterValidViewModelProperties(sourceViewModelProperties,
                        binderInputValueType);
                var sourceViewModelValidPropertyNames = sourceViewModelValidProperties.Select(x => x.Name).ToArray();
                
                _searchProvider.Init(sourceViewModelValidPropertyNames, newPropertyNameSelected =>
                {
                    _viewModelPropertyNameProperty.stringValue =
                        newPropertyNameSelected == MVVMConstants.NONE ? null : newPropertyNameSelected;
                    serializedObject.ApplyModifiedProperties();
                });
                
                var mousePos = Event.current.mousePosition;
                mousePos.Set(mousePos.x, mousePos.y);
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(mousePos), 250), _searchProvider);
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawSourceBinderProperty()
        {
            var oldSourceBinder = _sourceBinderProperty.objectReferenceValue;

            EditorGUILayout.PropertyField(_sourceBinderProperty);

            var newSourceBinder = _sourceBinderProperty.objectReferenceValue as ObservableBinder;

            if (newSourceBinder != null && (newSourceBinder.OutputType != _binder.InputType || ReferenceEquals(newSourceBinder, _binder)))
            {
                _sourceBinderProperty.objectReferenceValue = oldSourceBinder;
                throw new Exception(
                    $"Not valid binder source. Output type of the source binder must be {_binder.InputType} and mustn't refer to itself");
            }
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
    }
}