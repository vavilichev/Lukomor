using UnityEditor;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    public static class ViewEditorOnReloadValidationHandler
    {
        static ViewEditorOnReloadValidationHandler()
        {
            Undo.undoRedoPerformed += OnUndoRedo;
            PrefabUtility.prefabInstanceReverted += OnPrefabReverted;
        }

        private static void OnUndoRedo()
        {
            ValidateAllSceneViews();
        }
        
        private static void OnPrefabReverted(GameObject instance)
        {
            ValidateAllSceneViews();
        }
        
        [InitializeOnLoadMethod]
        [MenuItem("Lukomor/Views/Check All Scene Views Setup", false, 1)]
        private static void ValidateAllSceneViews()
        {
            var allSceneViews = Object.FindObjectsByType<View>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var view in allSceneViews)
            {
                view.ValidateViewModelSetup();
            }
            
        }
    }
}