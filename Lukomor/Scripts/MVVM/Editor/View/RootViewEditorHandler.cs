using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    public class RootViewEditorHandler : ViewEditorHandler
    {
        private readonly SerializedObject _serializedObject;
        private readonly StringListSearchProvider _searchProvider;
        private readonly View _view;

        /// <summary>
        /// Selected (by property) View Model Type Full Name
        /// </summary>
        private readonly SerializedProperty _viewModelTypeFullName;
        
        // help
        private string _previousViewModelSelected;

        public RootViewEditorHandler(SerializedObject serializedObject, StringListSearchProvider searchProvider, View view) : base(serializedObject, view)
        {
            _serializedObject = serializedObject;
            _searchProvider = searchProvider;
            _view = view;
            _viewModelTypeFullName = serializedObject.FindProperty(nameof(_viewModelTypeFullName));
            _previousViewModelSelected = _viewModelTypeFullName.stringValue;
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

            _searchProvider.Init(viewModelTypeFullNames, newViewModelTypeFullNameSelected =>
            {
                _viewModelTypeFullName.stringValue =
                    newViewModelTypeFullNameSelected == MVVMConstants.NONE ? null : newViewModelTypeFullNameSelected;
                _serializedObject.ApplyModifiedProperties();

                if (_previousViewModelSelected != newViewModelTypeFullNameSelected)
                {
                    _previousViewModelSelected = newViewModelTypeFullNameSelected;
                    _view.HandleParentViewModelChanging();
                }
                
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