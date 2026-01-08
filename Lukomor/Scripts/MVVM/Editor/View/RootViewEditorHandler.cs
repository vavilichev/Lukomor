using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    public class RootViewEditorHandler : ViewEditorHandler
    {
        private readonly SerializedObject _serializedObject;
        private readonly StringListSearchProvider _searchProvider;

        /// <summary>
        /// Selected (by property) View Model Type Full Name
        /// </summary>
        private readonly SerializedProperty _viewModelTypeFullName;

        public RootViewEditorHandler(SerializedObject serializedObject, StringListSearchProvider searchProvider, View view) : base(serializedObject, view)
        {
            _serializedObject = serializedObject;
            _searchProvider = searchProvider;
            _viewModelTypeFullName = serializedObject.FindProperty(nameof(_viewModelTypeFullName));
        }
        
        public void DrawEditor()
        {
            DrawEditorForParentView();
            DrawDebug();

            //DrawOpenViewModelButton(_view.ViewModelTypeFullName);
        }
        
        private void DrawEditorForParentView()
        {
            var viewModelTypeFullNames = ViewModelsDB.AllViewModelTypeFullNames;

            _searchProvider.Init(viewModelTypeFullNames, viewModelTypeFullName =>
            {
                _viewModelTypeFullName.stringValue = viewModelTypeFullName == MVVMConstants.NONE ? null : viewModelTypeFullName;
                _serializedObject.ApplyModifiedProperties();
                
                CheckSubViews();
            });
                
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(MVVMConstants.VIEW_MODEL);

            var displayName = string.IsNullOrEmpty(_viewModelTypeFullName.stringValue)
                ? MVVMConstants.NONE
                : ViewModelsEditorUtility.ToShortName(_viewModelTypeFullName.stringValue);
            
            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                var mousePos = Event.current.mousePosition;
                mousePos.Set(mousePos.x - 200, mousePos.y);
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(mousePos), 400), _searchProvider);
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }
}