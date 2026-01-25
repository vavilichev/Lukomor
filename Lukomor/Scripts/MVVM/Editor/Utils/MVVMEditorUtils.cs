using UnityEditor;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    public static class MVVMEditorUtils
    {
        public static void DrawScriptTitle(MonoBehaviour script)
        {
            GUI.enabled = false;
            var type = script.GetType();
            EditorGUILayout.ObjectField(MVVMConstants.SCRIPT, MonoScript.FromMonoBehaviour(script), type, false);
            GUI.enabled = true;
        }
    }
}