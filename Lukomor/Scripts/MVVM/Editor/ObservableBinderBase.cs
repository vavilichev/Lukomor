using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    public abstract class ObservableBinderBase : BinderEditor
    {
        protected abstract SerializedProperty _propertyName { get; set; }

        protected override void DrawProperties()
        {
            DrawPropertyNames();
        }

        private void DrawPropertyNames()
        {
            var viewModelType = GetViewModelType(ViewModelTypeFullName.stringValue);
            
            if (viewModelType == null)
            {
                EditorGUILayout.HelpBox($"Could not find ViewModel. Maybe you forget to chose one, or maybe you renamed the ViewModel. Registered name: {ViewModelTypeFullName.stringValue}", MessageType.Error);
                return;
            }

            var allProperties = viewModelType.GetProperties().Where(p => p.PropertyType.IsGenericType);
            var validProperties = allProperties.Where(p => IsValidProperty(p.PropertyType));
            var validPropertyNames = validProperties.Select(p => p.Name);
            var provider = CreateInstance<StringListSearchProvider>();
            var options = new List<string> { MVVMConstants.NONE };
            options.AddRange(validPropertyNames);
            
            provider.Init(options.ToArray(), result =>
            {
                _propertyName.stringValue = result == MVVMConstants.NONE ? null : result;

                serializedObject.ApplyModifiedProperties();
            });
            
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(MVVMConstants.PROPERTY_NAME);

            var displayName = string.IsNullOrEmpty(_propertyName.stringValue)
                ? MVVMConstants.NONE
                : _propertyName.stringValue;
            
            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), provider);
            }
            
            EditorGUILayout.EndHorizontal();
            
            if (!IsValidPropertyName(_propertyName.stringValue, viewModelType))
            {
                EditorGUILayout.HelpBox($"Property Name ({_propertyName.stringValue}) not found in ViewModel: {viewModelType.Name}. Please choose correct property name.", MessageType.Warning);
            }
        }

        protected abstract bool IsValidProperty(Type propertyType);
    }
}