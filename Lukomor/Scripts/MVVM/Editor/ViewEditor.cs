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
        private SerializedProperty _childBinders;
        private readonly Dictionary<string, string> _viewModelNames = new();

        private void OnEnable()
        {
            _viewModelTypeFullName = serializedObject.FindProperty(nameof(_viewModelTypeFullName));
            _childBinders = serializedObject.FindProperty(nameof(_childBinders));
        }

        public override void OnInspectorGUI()
        {
            DefineAllViewModels();

            var provider = CreateInstance<StringListSearchProvider>();
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

            DrawDebug();
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

        private string ToShortName(string viewModelTypeFullName)
        {
            var viewModelType = Type.GetType(viewModelTypeFullName);
            
            return viewModelType == null ? NONE : viewModelType.Name;
        }

        private void DrawDebug()
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(_childBinders);
            GUI.enabled = true;
        }
    }
}