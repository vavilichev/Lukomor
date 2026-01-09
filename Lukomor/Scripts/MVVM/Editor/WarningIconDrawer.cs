using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    [InitializeOnLoad]
    public static class WarningIconDrawer
    {
        private static readonly List<int> _instanceIDs = new();
        
        public static readonly Texture2D WarningIcon;
        
        static WarningIconDrawer()
        {
            WarningIcon = EditorGUIUtility.IconContent("console.warnicon.sml").image as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawIcon;
        }

        public static void DrawIcon(int instanceID, Rect rect)
        {
            if (!_instanceIDs.Contains(instanceID))
            {
                return;
            }
            
            var iconRect = new Rect(
                rect.xMax - 18,
                rect.y + 1,
                16,
                16
            );

            GUI.DrawTexture(iconRect, WarningIcon);
        }

        public static void AddWarningView(int instanceID)
        {
            if (!_instanceIDs.Contains(instanceID))
            {
                _instanceIDs.Add(instanceID);
                EditorApplication.RepaintHierarchyWindow();
            }
        }

        public static void RemoveWarningView(int instanceID)
        {
            _instanceIDs.Remove(instanceID);
            EditorApplication.RepaintHierarchyWindow();
        }
    }
}