#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lukomor.MVVM.Binders;
using Lukomor.Reactive;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor.Binders
{
    [CustomEditor(typeof(ObservableCollectionBinder), true)]
    public class ObservableCollectionBinderEditor : UnityEditor.Editor
    {
        private const string PROP_SOURCE_VIEW = "_sourceView";
        private const string PROP_VIEW_MODEL_PROPERTY_NAME = "_viewModelPropertyName";
        
        private static readonly HashSet<string> SetupFields = new()
        {
            PROP_SOURCE_VIEW,
            PROP_VIEW_MODEL_PROPERTY_NAME,
        };
        
        private StringListSearchProvider _searchProvider;
        private ObservableCollectionBinder _binder;
        private SerializedProperty _sourceViewProperty;
        private SerializedProperty _viewModelPropertyNameProperty;

        protected void OnEnable()
        {
            _searchProvider = CreateInstance<StringListSearchProvider>();
            _binder = (ObservableCollectionBinder)target;
            _sourceViewProperty = serializedObject.FindProperty(PROP_SOURCE_VIEW);
            _viewModelPropertyNameProperty = serializedObject.FindProperty(PROP_VIEW_MODEL_PROPERTY_NAME);
        }

        public sealed override void OnInspectorGUI()
        {
            var isHeaderDrawnCompletely = TryDrawCustomHeader();

            if (!isHeaderDrawnCompletely)
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
                EditorGUILayout.HelpBox("Please, select Source View, first, please", MessageType.Warning);
                return false;
            }

            var sourceView = _sourceViewProperty.objectReferenceValue as View;
            var sourceViewModelTypeFullName = sourceView?.ViewModelTypeFullName;
            if (string.IsNullOrEmpty(sourceViewModelTypeFullName))
            {
                // view model type must be selected
                EditorGUILayout.HelpBox("Source View doesn't have the ViewModel selection. Check View, please.",
                                        MessageType.Warning);
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
                    FilterValidProperties(sourceViewModelProperties, binderInputValueType);
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

        private static PropertyInfo[] FilterValidProperties(PropertyInfo[] allProperties, Type binderInputValueType)
        {
            var validProperties = allProperties.Where(p =>
            {
                var propertyType = p.PropertyType;
                if (!propertyType.IsPublic || !propertyType.IsGenericType || propertyType.IsArray)
                {
                    return false;
                }
                
                var isDirectlyObservableCollection = propertyType.GetGenericTypeDefinition() == typeof(IReadOnlyReactiveCollection<>);
                if (!isDirectlyObservableCollection)
                {
                    var interfaces = propertyType.GetInterfaces();
                    var isInheritedByObservableCollection =
                        interfaces.FirstOrDefault(i => i.IsGenericType &&
                                                       i.GetGenericTypeDefinition() == typeof(IReadOnlyReactiveCollection<>)) != null;
                    if (!isInheritedByObservableCollection)
                    {
                        return false;
                    }
                }

                var genericArgs = propertyType.GetGenericArguments();
                if (genericArgs.Length != 1)
                {
                    return false;
                }

                var genericArgumentType = genericArgs[0];
                var result = binderInputValueType.IsAssignableFrom(genericArgumentType);
                return result;
            }).ToArray();

            return validProperties;
        }

        private void CheckValidation()
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
        }
    }
}
#endif