using Lukomor.MVVM.Binders;
using UnityEditor;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    public class BinderEditorOnReloadValidationHandler
    {
        static BinderEditorOnReloadValidationHandler()
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
        [MenuItem("Lukomor/Views/Check All Scene Binders Setup", false, 2)]
        private static void ValidateAllSceneViews()
        {
            var allSceneBinders = Object.FindObjectsByType<ObservableBinder>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var binder in allSceneBinders)
            {
                binder.CheckValidation();
            }
            
            var allCommandBinders = Object.FindObjectsByType<CommandBinderBase>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var binder in allCommandBinders)
            {
                binder.CheckValidation();
            }
        }
    }
}