using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    public abstract class MethodBinderEditor : BinderEditor
    {
        private SerializedProperty _propertyName;

        protected override void OnEnable()
        {
            base.OnEnable();

            _propertyName = serializedObject.FindProperty(nameof(_propertyName));
        }

        protected override void DrawProperties()
        {
            var viewModelType = GetViewModelType(ViewModelTypeFullName.stringValue);
            
            if (viewModelType == null)
            {
                EditorGUILayout.HelpBox($"Could not find ViewModel. Maybe you forget to chose one, or maybe you renamed the ViewModel. Registered name: {ViewModelTypeFullName.stringValue}", MessageType.Error);
                return;
            }
            
            var allMethods = GetMethodsInfo();
            var allMethodNames = allMethods.Select(m => m.Name);
            var provider = CreateInstance<StringListSearchProvider>();
            var options = new List<string> { MVVMConstants.NONE };
            options.AddRange(allMethodNames);

            provider.Init(options.ToArray(), result =>
            {
                _propertyName.stringValue = result == MVVMConstants.NONE ? null : result;

                serializedObject.ApplyModifiedProperties();
            });

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(MVVMConstants.METHOD_NAME);

            var displayName = string.IsNullOrEmpty(_propertyName.stringValue)
                ? MVVMConstants.NONE
                : _propertyName.stringValue;

            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                    provider);
            }

            EditorGUILayout.EndHorizontal();
            
            if (!IsValidMethodName(_propertyName.stringValue, viewModelType))
            {
                EditorGUILayout.HelpBox($"Property Name ({_propertyName.stringValue}) not found in ViewModel: {viewModelType.Name}. Please choose correct property name.", MessageType.Warning);
            }
        }

        protected abstract IEnumerable<MethodInfo> GetMethodsInfo();
    }
}