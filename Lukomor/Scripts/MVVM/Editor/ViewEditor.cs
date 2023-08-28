using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private readonly Dictionary<string, string> _viewModelNames = new();
        private Assembly _assembly;

        private void OnEnable()
        {
            _assembly = Assembly.GetAssembly(typeof(IViewModel));
            _viewModelTypeFullName = serializedObject.FindProperty(nameof(_viewModelTypeFullName));
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
                : _viewModelTypeFullName.stringValue;
            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), provider);
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private void DefineAllViewModels()
        {
            _viewModelNames.Clear();
            _viewModelNames[NONE] = null;

            var allViewModelsTypes = _assembly.GetTypes()
                .Where(myType => 
                    myType.IsClass 
                    && !myType.IsAbstract 
                    && typeof(IViewModel).IsAssignableFrom(myType));
            
            foreach (var viewModelsType in allViewModelsTypes)
            {
                _viewModelNames[viewModelsType.Name] = viewModelsType.FullName;
            }
        }
    }
}