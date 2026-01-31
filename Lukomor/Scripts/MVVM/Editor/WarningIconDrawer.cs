using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    [InitializeOnLoad]
    public static class WarningIconDrawer
    {
        private static readonly List<int> _instanceIDs = new();
        private static readonly HashSet<int> _allGameObjectInstanceIds = new();
        private static readonly Dictionary<int, HashSet<int>> _instanceIdsMap = new(); // gameObjectInstanceId, List<scriptInstanceId>
        
        public static readonly Texture2D WarningIcon;
        
        static WarningIconDrawer()
        {
            WarningIcon = EditorGUIUtility.IconContent("console.warnicon.sml").image as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawIcon;
        }

        public static void DrawIcon(int instanceID, Rect rect)
        {
            if (!_allGameObjectInstanceIds.Contains(instanceID))
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

        public static void AddWarning(int gameObjectInstanceId, int scriptInstanceId)
        {
            if (!_instanceIdsMap.TryGetValue(gameObjectInstanceId, out var scriptInstanceIds))
            {
                scriptInstanceIds = new HashSet<int>();
                _instanceIdsMap[gameObjectInstanceId] = scriptInstanceIds;
            }

            scriptInstanceIds.Add(scriptInstanceId);
            _allGameObjectInstanceIds.Add(gameObjectInstanceId);
            EditorApplication.RepaintHierarchyWindow();
        }

        public static void RemoveWarning(int gameObjectInstanceId, int scriptInstanceId)
        {
            if (!_instanceIdsMap.TryGetValue(gameObjectInstanceId, out var scriptInstanceIds))
            {
                return;
            }

            if (scriptInstanceIds.Contains(scriptInstanceId))
            {
                scriptInstanceIds.Remove(scriptInstanceId);

                if (scriptInstanceIds.Count == 0 && _allGameObjectInstanceIds.Contains(gameObjectInstanceId))
                {
                    _allGameObjectInstanceIds.Remove(gameObjectInstanceId);
                    EditorApplication.RepaintHierarchyWindow();
                }
            }
        }

        public static void ClearGameObject(int gameObjectInstanceId)
        {
            if (_instanceIdsMap.TryGetValue(gameObjectInstanceId, out var scriptInstanceIds))
            {
                scriptInstanceIds.Clear();
            }

            if (_allGameObjectInstanceIds.Contains(gameObjectInstanceId))
            {
                _allGameObjectInstanceIds.Remove(gameObjectInstanceId);
            }
        }
    }
}