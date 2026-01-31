using Lukomor.MVVM.Binders;
using Lukomor.MVVM.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

namespace Lukomor._Lukomor.Lukomor.Scripts.MVVM.Editor.Binders
{
    [CustomEditor(typeof(ViewModelToViewDirectRefMapper), true)]
    public class ViewModelToViewDirectRefMapperEditor : UnityEditor.Editor
    {
        private const float SPACE = 6;
        
        private StringListSearchProvider _searchProvider;
        private ViewModelToViewDirectRefMapper _mapper;
        private SerializedProperty _mappings;
        private ReorderableList _list;

        protected void OnEnable()
        {
            _searchProvider = CreateInstance<StringListSearchProvider>();
            _mapper = (ViewModelToViewDirectRefMapper)target;
            _mappings = serializedObject.FindProperty("_mappings");
            _list = new ReorderableList(serializedObject, _mappings, true, true, true, true)
            {
                drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "Mappings:"); },

                drawElementCallback = (rect, index, _, _) =>
                {
                    var titleLabelRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.LabelField(titleLabelRect, $"Element {index}:");

                    EditorGUI.indentLevel++;
                    
                    var element = _mappings.GetArrayElementAtIndex(index);
                    var labelRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + SPACE, 150,
                                             EditorGUIUtility.singleLineHeight);
                    EditorGUI.LabelField(labelRect, "ViewModel:");
                    
                    var viewModelFullTypeName = element.FindPropertyRelative("_viewModelFullTypeName");
                    var displayName = string.IsNullOrEmpty(viewModelFullTypeName.stringValue)
                        ? MVVMConstants.NONE
                        : ViewModelsEditorUtility.ConvertViewModelType(viewModelFullTypeName.stringValue).Name;
                    var buttonWidth = rect.width * 0.57f;
                    var buttonRect = new Rect(rect.xMax - buttonWidth, labelRect.y, buttonWidth,
                                              EditorGUIUtility.singleLineHeight);
                    if (GUI.Button(buttonRect, displayName, EditorStyles.popup))
                    {
                        OpenSearchWindow(element);
                    }

                    var prefabProperty = element.FindPropertyRelative("_prefab");
                    var prefabPropertyRect = new Rect(rect.x, rect.y + (EditorGUIUtility.singleLineHeight + SPACE) * 2,
                                                      rect.width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(prefabPropertyRect, prefabProperty);

                    EditorGUI.indentLevel--;
                },

                elementHeightCallback = _ => (EditorGUIUtility.singleLineHeight + SPACE) * 3
            };
        }

        public sealed override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            MVVMEditorLayout.DrawScriptTitle(_mapper);
            
            _list.DoLayoutList(); // отрисовываем массив
            serializedObject.ApplyModifiedProperties();
        }
        
        private void OpenSearchWindow(SerializedProperty element)
        {
            var targetObject = element.serializedObject.targetObject;
            var propertyPath = element.FindPropertyRelative("_viewModelFullTypeName").propertyPath;

            _searchProvider.Init(ViewModelsDB.AllViewModelTypeFullNames, result =>
            {
                var so = new SerializedObject(targetObject);
                var prop = so.FindProperty(propertyPath);
                prop.stringValue = result == MVVMConstants.NONE ? null : result;
                so.ApplyModifiedProperties();
            });

            // Открываем SearchWindow в центре экрана (без GUIUtility.mousePosition!)
            SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), _searchProvider);
        }
    }
}