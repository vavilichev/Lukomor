using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    [CustomEditor(typeof(ButtonActionBinder))]
    public class ButtonActionBinderEditor : UnityEditor.Editor
    {
        private const string None = "None";
        
        private SerializedProperty _button;
        private SerializedProperty _methodNameProperty;

        private List<string> _options = new List<string>();

        private void OnEnable()
        {
            _button = serializedObject.FindProperty("_button");
            _methodNameProperty = serializedObject.FindProperty("_methodName");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_button);

            serializedObject.ApplyModifiedProperties();
            
            var parentViewGo = ((MonoBehaviour)serializedObject.targetObject).gameObject;
            var parentView = parentViewGo.GetComponentInParent<IView>();
            var viewModelType = parentView.ViewModelType;

            UpdateOptions(viewModelType);

            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Method");

            var displayName = string.IsNullOrEmpty(_methodNameProperty.stringValue)
                ? None
                : _methodNameProperty.stringValue;

            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                var provider = CreateInstance<StringListSearchProvider>();
                provider.Init(_options.ToArray(), value =>
                {
                    _methodNameProperty.stringValue = value == None ? null : value;
                    
                    serializedObject.ApplyModifiedProperties();
                });
                
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), provider);
            }
            
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button($"Highlight parent VM ({viewModelType.Name})"))
            {
                EditorGUIUtility.PingObject(parentView.gameObject);
            }
        }

        private void UpdateOptions(Type viewModelType)
        {
            _options.Clear();
            _options.Add(None);

            var allMethodsPublic = viewModelType.GetMethods()
                .Where(m =>
                    m.IsPublic 
                    && m.ReturnType == typeof(void) 
                    && m.GetParameters().Length == 0
                    && !m.IsSpecialName);
            
            _options.AddRange(allMethodsPublic.Select(a => a.Name));
        }
    }
}