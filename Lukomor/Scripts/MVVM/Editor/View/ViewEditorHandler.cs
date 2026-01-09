using System.Linq;
using UnityEditor;
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
        
        // help
        private View _previousParentViewValue;
        
        public ViewEditorHandler(SerializedObject serializedObject, View view)
        {
            _serializedObject = serializedObject;
            _view = view;
            _showEditorLogs = serializedObject.FindProperty(nameof(_showEditorLogs));
            _isParentView = serializedObject.FindProperty(nameof(_isParentView));
            _parentView = serializedObject.FindProperty(nameof(_parentView));
            _previousParentViewValue = _parentView.objectReferenceValue as View;
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
            EditorGUILayout.PropertyField(_parentView);
            if (EditorGUI.EndChangeCheck())
            {
                var newParentView = _parentView.objectReferenceValue as View;
                if (!ReferenceEquals(newParentView, _previousParentViewValue))
                {
                    _view.HandleParentViewModelChanging();
                    _previousParentViewValue = newParentView;
                }
            }
            _serializedObject.ApplyModifiedProperties();
        }
    }
}