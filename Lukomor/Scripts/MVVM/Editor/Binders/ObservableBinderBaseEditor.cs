using System;
using System.Collections.Generic;
using System.Linq;
using Lukomor.MVVM.Binders;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor.Binders
{
    [CustomEditor(typeof(ObservableBinderBase), true)]
    public class ObservableBinderBaseEditor : UnityEditor.Editor
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
        private ObservableBinderBase _binder;
        private SerializedProperty _bindingTypeProperty;
        private SerializedProperty _sourceViewProperty;
        private SerializedProperty _viewModelPropertyNameProperty;
        private SerializedProperty _sourceBinderProperty;

        protected void OnEnable()
        {
            _searchProvider = CreateInstance<StringListSearchProvider>();
            _binder = (ObservableBinderBase)target;
            _bindingTypeProperty = serializedObject.FindProperty(PROP_BINDING_TYPE);
            _sourceViewProperty = serializedObject.FindProperty(PROP_SOURCE_VIEW);
            _viewModelPropertyNameProperty = serializedObject.FindProperty(PROP_VIEW_MODEL_PROPERTY_NAME);
            _sourceBinderProperty = serializedObject.FindProperty(PROP_SOURCE_BINDER);
        }

        public sealed override void OnInspectorGUI()
        {
            var hasHeaderDrawnCompletely = TryDrawCustomHeader();

            if (!hasHeaderDrawnCompletely)
            {
                return;
            }
            
            MVVMEditorLayout.DrawInheritedProperties(serializedObject, SetupFields);
            CheckValidation();

            serializedObject.ApplyModifiedProperties();
        }

        private bool TryDrawCustomHeader()
        {
            MVVMEditorLayout.DrawScriptTitle(_binder);
            DrawBindingTypeProperty();
            
            if (_binder.BindingType == BindingType.View)
            {
                return TryDrawSourceViewSetup();
            }

            return TryDrawSourceBinderProperty();
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

            serializedObject.ApplyModifiedProperties();
            MVVMValidator.RequestValidation();
        }
        
        private bool TryDrawSourceViewSetup()
        {
            DrawSourceViewProperty();
            var result = TryDrawSourceViewPropertyNameProperty();
            return result;
        }
        
        private void DrawSourceViewProperty()
        {
            EditorGUILayout.PropertyField(_sourceViewProperty);
            serializedObject.ApplyModifiedProperties();
        }
        
        private bool TryDrawSourceViewPropertyNameProperty()
        {
            if (_sourceViewProperty.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Select Source View, Please", MessageType.Warning);
                return false;
            }

            var sourceView = _sourceViewProperty.objectReferenceValue as View;
            var sourceViewModelTypeFullName = sourceView?.ViewModelTypeFullName;
            if (string.IsNullOrEmpty(sourceViewModelTypeFullName))
            {
                // view model type must be selected
                EditorGUILayout.HelpBox("Check Source View setup. ViewModel must be selected", MessageType.Warning);
                return false;
            }

            DrawSelectViewModelPropertyLine(sourceViewModelTypeFullName);
            return true;
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
                    
                    MVVMValidator.RequestValidation();
                });
                
                var mousePos = Event.current.mousePosition;
                mousePos.Set(mousePos.x, mousePos.y);
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(mousePos), 250), _searchProvider);
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private bool TryDrawSourceBinderProperty()
        {
            var oldSourceBinder = _sourceBinderProperty.objectReferenceValue;

            EditorGUILayout.PropertyField(_sourceBinderProperty);

            var newSourceBinder = _sourceBinderProperty.objectReferenceValue as ObservableBinderBase;

            if (newSourceBinder != null && (newSourceBinder.OutputType != _binder.InputType || ReferenceEquals(newSourceBinder, _binder)))
            {
                _sourceBinderProperty.objectReferenceValue = oldSourceBinder;
                throw new Exception(
                    $"Not valid binder source. Output type of the source binder must be {_binder.InputType} and mustn't refer to itself");
            }

            return true;
        }

        private void CheckValidation()
        {
            var bindingType = (BindingType)_bindingTypeProperty.enumValueIndex;
            if (bindingType == BindingType.View)
            {
                var sourceView = _sourceViewProperty.objectReferenceValue as View;
                if (sourceView == null)
                {
                    EditorGUILayout.HelpBox("No View selected", MessageType.Warning);
                    return;
                }

                var propertyName = _viewModelPropertyNameProperty.stringValue;
                if (string.IsNullOrEmpty(propertyName))
                {
                    EditorGUILayout.HelpBox("View Model Property wasn't selected", MessageType.Warning);
                }
                return;
            }

            if (bindingType == BindingType.Binder)
            {
                var sourceBinder = _sourceBinderProperty.objectReferenceValue as ObservableBinderBase;
                if (sourceBinder == null)
                {
                    EditorGUILayout.HelpBox("No binder selected", MessageType.Warning);
                }
            }
        }
    }
}