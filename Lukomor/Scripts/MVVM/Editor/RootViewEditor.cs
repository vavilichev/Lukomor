using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    [CustomEditor(typeof(RootView))]
    public class RootViewEditor : UnityEditor.Editor
    {
        private const string None = nameof(None);
        
        private SerializedProperty _viewModelPath;
        private Dictionary<string, string> _viewModelTypes = new();

        private void OnEnable()
        {
            _viewModelPath = serializedObject.FindProperty(nameof(_viewModelPath));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            DrawViewModelSearchPanel();
        }

        private void DrawViewModelSearchPanel()
        {
            _viewModelTypes.Clear();
            _viewModelTypes.Add(None, string.Empty);
            
            var allViewModelTypes = Assembly.GetAssembly(typeof(IViewModel))
                .GetTypes()
                .Where(myType => myType.IsClass
                                 && !myType.IsAbstract
                                 && typeof(IViewModel).IsAssignableFrom(myType)).ToArray();

            foreach (var viewModelTypeItem in allViewModelTypes)
            {
                var shortName = viewModelTypeItem.Name;
                var path = viewModelTypeItem.FullName;

                _viewModelTypes[shortName] = path;
            }

            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("ViewModel:");

            var viewModelType = Type.GetType(_viewModelPath.stringValue);
            var displayName = viewModelType == null ? None : viewModelType.Name;
            var options = _viewModelTypes.Keys.ToArray();

            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                var provider = CreateInstance<StringListSearchProvider>();
                provider.Init(options, value =>
                {
                    _viewModelPath.stringValue = _viewModelTypes[value];
                    serializedObject.ApplyModifiedProperties();
                });
                
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), provider);
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }
}