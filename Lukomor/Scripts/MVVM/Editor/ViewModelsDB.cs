using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    public static class ViewModelsDB
    {
        private static TypeCache.TypeCollection _allViewModelTypes;

        public static IEnumerable<Type> AllViewModelTypes => _allViewModelTypes;

        [InitializeOnLoadMethod]
        [MenuItem("Lukomor/View Models/Force Update View Models", false, 1)]
        private static void UpdateViewModelsDB()
        {
            _allViewModelTypes = TypeCache.GetTypesDerivedFrom<IViewModel>();
            Debug.Log("UpdateViewModelsDB");
        }
        
        [MenuItem("Lukomor/Auto Recompile/Enable", false, 1)]
        public static void EnableAutoRecompile()
        {
            EditorApplication.UnlockReloadAssemblies();
            Debug.Log("Lukomor: Auto Recompilation Enabled");
        }

        [MenuItem("Lukomor/Auto Recompile/Disable", false, 2)]
        public static void DisableAutoRecompile()
        {
            EditorApplication.LockReloadAssemblies();
            Debug.Log("Lukomor: Auto Recompilation Disabled");
        }

        [MenuItem("Lukomor/Auto Recompile/Force Recompile", false, 3)]
        public static void ForceRecompile()
        {
            AssetDatabase.Refresh();
            Debug.Log("Lukomor: Manual Recompilation Triggered");
        }
    }
}