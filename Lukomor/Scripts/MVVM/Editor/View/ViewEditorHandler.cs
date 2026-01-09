using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    public abstract class ViewEditorHandler
    {
        private readonly SerializedObject _serializedObject;
        private readonly View _view;
        private readonly SerializedProperty _showEditorLogs;
        private readonly SerializedProperty _isParentView;
        private readonly SerializedProperty _parentView;
        private readonly SerializedProperty _subViews;
        private readonly SerializedProperty _childBinders;

        public ViewEditorHandler(SerializedObject serializedObject, View view)
        {
            _serializedObject = serializedObject;
            _view = view;
            _showEditorLogs = serializedObject.FindProperty(nameof(_showEditorLogs));
            _isParentView = serializedObject.FindProperty(nameof(_isParentView));
            _parentView = serializedObject.FindProperty(nameof(_parentView));
            _subViews = serializedObject.FindProperty(nameof(_subViews));
            _childBinders = serializedObject.FindProperty(nameof(_childBinders));
        }

        protected void DrawDebug()
        {
            _showEditorLogs.boolValue = EditorGUILayout.Foldout(_showEditorLogs.boolValue, "Logs");
            _serializedObject.ApplyModifiedProperties();

            if (_showEditorLogs.boolValue)
            {
                EditorGUI.indentLevel++;
                GUI.enabled = false;
                EditorGUILayout.PropertyField(_isParentView);
                EditorGUILayout.PropertyField(_subViews);
                EditorGUILayout.PropertyField(_childBinders);
                GUI.enabled = true;
                EditorGUI.indentLevel--;
            }
        }

        public void CheckSubViews()
        {
            var allSubViews = _view.gameObject.GetComponentsInChildren<View>(true)
                .Where(c => !ReferenceEquals(c, _view));

            foreach (var subView in allSubViews)
            {
                subView.HandleParentViewModelChanging();
            }
        }

        protected void DrawParentViewField()
        {
            var isParentView = _isParentView.boolValue;
            var isInPrefabMode = PrefabStageUtility.GetPrefabStage(_view.gameObject) != null;

            if (!isInPrefabMode && isParentView)
            {
                return; // do not draw parent field, if parent doesn't exist and we are not in the prefab mode
            }

            var oldParentViewValue = _parentView.objectReferenceValue as View;
            EditorGUILayout.PropertyField(_parentView);
            _serializedObject.ApplyModifiedProperties();

            var newParentView = _parentView.objectReferenceValue as View;
            if (!ReferenceEquals(newParentView, oldParentViewValue))
            {
                if (newParentView != null)
                {
                    var allParentViews = _view.gameObject.GetComponentsInParent<View>().Where(v => !ReferenceEquals(v, _view));
                    if (!allParentViews.Contains(newParentView))
                    {
                        _parentView.objectReferenceValue = null;
                        _serializedObject.ApplyModifiedProperties();
                        Debug.LogError($"ViewEditor [{_view.gameObject.name}]: Parent view must be higher in the hierarchy.");
                        return;
                    }
                    
                    _view.HandleParentViewModelChanging();
                }
            }
        }
    }
}