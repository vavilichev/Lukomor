using UnityEditor;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    public static class ViewEditorOnReloadValidationHandler
    {
        [InitializeOnLoadMethod]
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