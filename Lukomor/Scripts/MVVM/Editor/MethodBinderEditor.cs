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
            var allMethods = GetMethodsInfo();
            var allMethodNames = allMethods.Select(m => m.Name);
            var provider = CreateInstance<StringListSearchProvider>();
            var options = new List<string> { NONE };
            options.AddRange(allMethodNames);

            provider.Init(options.ToArray(), result =>
            {
                _propertyName.stringValue = result == NONE ? null : result;

                serializedObject.ApplyModifiedProperties();
            });

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Method Name:");

            var displayName = string.IsNullOrEmpty(_propertyName.stringValue)
                ? NONE
                : _propertyName.stringValue;

            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                    provider);
            }

            EditorGUILayout.EndHorizontal();
        }

        protected abstract IEnumerable<MethodInfo> GetMethodsInfo();
    }
}