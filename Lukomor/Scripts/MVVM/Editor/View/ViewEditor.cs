using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    [CustomEditor(typeof(View))]
    public class ViewEditor : UnityEditor.Editor
    {
        private SerializedProperty _viewModelTypeFullName;
        private SerializedProperty _viewModelPropertyName;
        private SerializedProperty _isParentView;
        private SerializedProperty _childBinders;
        private SerializedProperty _subViews;
        private SerializedProperty _showEditorLogs;
        private View _view;        
        private SerializedProperty _parentView;
        private readonly Dictionary<string, string> _viewModelNames = new();
        private StringListSearchProvider _searchProvider;
        
        // help 
        private readonly List<View> _parentViews = new();
        private RootViewEditorHandler _rootViewEditorHandler;
        private SubViewEditorHandler _subViewEditorHandler;
        private View _parentViewPreviousValue;

        private void OnEnable()
        {
            _searchProvider = CreateInstance<StringListSearchProvider>();
            _view = (View)target;
            _viewModelTypeFullName = serializedObject.FindProperty(nameof(_viewModelTypeFullName));
            _viewModelPropertyName = serializedObject.FindProperty(nameof(_viewModelPropertyName));
            _isParentView = serializedObject.FindProperty(nameof(_isParentView));
            _childBinders = serializedObject.FindProperty(nameof(_childBinders));
            _subViews = serializedObject.FindProperty(nameof(_subViews));
            _parentView = serializedObject.FindProperty(nameof(_parentView));
            _parentViewPreviousValue = _parentView.objectReferenceValue as View;
            _showEditorLogs = serializedObject.FindProperty(nameof(_showEditorLogs));

            _subViewEditorHandler = new SubViewEditorHandler(serializedObject, _searchProvider, _view);
            _rootViewEditorHandler = new RootViewEditorHandler(serializedObject, _searchProvider, _view);
            
            //_view.ValidateParentingSetup();
        }

        public override void OnInspectorGUI()
        {
            DrawScriptTitle();
            DefineParentViews();
            AutoDefineParentView();
            
            var isParentViewExist = _parentViews.Count > 0; 

            if (isParentViewExist)
            {
                _subViewEditorHandler.DrawEditor();
            }
            else
            {
                _rootViewEditorHandler.DrawEditor();
            }
            
            DrawOpenViewModelButton();

            if (!_view.IsValidSetup())
            {
                DrawFixButton();
            }
        }
        
        private void DrawScriptTitle()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField(MVVMConstants.SCRIPT, MonoScript.FromMonoBehaviour((View)target), typeof(View), false);
            GUI.enabled = true;
        }
        
        private void DefineParentViews()
        {
            _parentViews.Clear();

            var parent = _view.transform.parent;
            if (parent != null)
            {
                var foundParentViews = parent
                    .GetComponentsInParent<View>()
                    .Where(c => !ReferenceEquals(c, _view));
                _parentViews.AddRange(foundParentViews);
            }
        }

        private void AutoDefineParentView()
        {
            if (_parentView.objectReferenceValue != null)
            {
                var parentViewRef = _parentView.objectReferenceValue as View;
                if (_parentViews.Contains(parentViewRef))
                {
                    return; // Existed parent view is valid
                }

                // Existed view is not valid, reset
                _parentView.objectReferenceValue = null;
                serializedObject.ApplyModifiedProperties();

                // If parent views exist, it means that we can redefine parent automatically, but warn user about it
                if (_parentViews.Count > 0)
                {
                    // reset view model, as it haw been changed
                    _viewModelTypeFullName.stringValue = null;
                    serializedObject.ApplyModifiedProperties();
                    
                    Debug.LogWarning(
                        "Parent View MUST be higher in the hierarchy to work properly. Trying to auto redefine parent view.", _view.gameObject);
                }
            }

            if (_parentViews.Count == 0)
            {
                _isParentView.boolValue = true;

                if (!string.IsNullOrEmpty(_viewModelPropertyName.stringValue))
                {
                    // means that it was sub view and became root view, reset all setup
                    _viewModelPropertyName.stringValue = null;
                    _viewModelTypeFullName.stringValue = null;
                    _rootViewEditorHandler.CheckSubViews();
                }

                serializedObject.ApplyModifiedProperties();
                return;
            }

            _isParentView.boolValue = false;
            _parentView.objectReferenceValue = _parentViews.First();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawOpenViewModelButton()
        {
            if (!string.IsNullOrEmpty(_viewModelTypeFullName.stringValue))
            {
                var viewModelType = ViewModelsEditorUtility.ConvertViewModelType(_viewModelTypeFullName.stringValue);
            
                if (viewModelType == null)
                {
                    return;
                }
            
                if (GUILayout.Button($"Open {viewModelType.Name}"))
                {
                    OpenScript(viewModelType.Name);
                }
            }
        }
        
        private void OpenScript(string typeName)
        {
            var guids = AssetDatabase.FindAssets($"t: script {typeName}");

            if (guids.Length > 0)
            {
                var scriptPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                
                EditorUtility.OpenWithDefaultApp(scriptPath);
            }
            else
            {
                Debug.LogError($"No script found for type: {typeName}");
            }
        }

        private void DrawFixButton()
        {
            EditorGUILayout.HelpBox("Some binders or sub views are missing. Please, fix it", MessageType.Warning);
            
            if (GUILayout.Button($"Fix"))
            {
                _view.Fix();
            }
        }
    }
}