using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    public static class MVVMEditorLayout
    {
        public static void DrawScriptTitle(MonoBehaviour script)
        {
            GUI.enabled = false;
            var type = script.GetType();
            EditorGUILayout.ObjectField(MVVMConstants.SCRIPT, MonoScript.FromMonoBehaviour(script), type, false);
            GUI.enabled = true;
        }

        public static void DrawInheritedProperties(SerializedObject serializedObject,
                                                   HashSet<string> excludedPropertyNames)
        {
            var iterator = serializedObject.GetIterator();
            EditorGUILayout.Space();

            var enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;

                if (iterator.name == "m_Script")
                    continue;

                if (excludedPropertyNames.Contains(iterator.name))
                    continue;

                EditorGUILayout.PropertyField(iterator, true);
            }
        }
    }
}