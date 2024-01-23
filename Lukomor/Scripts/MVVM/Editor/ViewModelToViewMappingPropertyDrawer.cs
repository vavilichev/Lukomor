using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    [CustomPropertyDrawer(typeof(ViewModelToViewMapping))]
    public class ViewModelToViewMappingPropertyDrawer : PropertyDrawer
    {
        private readonly Dictionary<string, string> _viewModelNames = new();
        private readonly GUIContent _viewModelLabelGUIContent = new("ViewModel:");
        private readonly GUIContent _prefabViewGUIContent = new("Prefab View");
        private TypeCache.TypeCollection _cachedViewModelTypes;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _cachedViewModelTypes = TypeCache.GetTypesDerivedFrom<IViewModel>();
            
            var viewModelTypeFullName = property.FindPropertyRelative("ViewModelTypeFullName");
            var prefabView = property.FindPropertyRelative("PrefabView");
            
            ViewModelsEditorUtility.DefineAllViewModels(_viewModelNames);

            EditorGUI.BeginProperty(position, label, property);
            
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            EditorGUI.indentLevel += 1;

            var viewModelTypeLabelRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 5, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight);
            var viewModelTypeButtonRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y + EditorGUIUtility.singleLineHeight + 5, EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            var prefabViewRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2 + 10, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight);

            var provider = ScriptableObject.CreateInstance<StringListSearchProvider>();
            var options = _viewModelNames.Keys.ToArray();

            provider.Init(options, result =>
            {
                viewModelTypeFullName.stringValue = _viewModelNames[result];
                
                property.serializedObject.ApplyModifiedProperties();
            });
               
            EditorGUI.PrefixLabel(viewModelTypeLabelRect, GUIUtility.GetControlID(FocusType.Passive), _viewModelLabelGUIContent);
            var buttonDisplayName = string.IsNullOrEmpty(viewModelTypeFullName.stringValue)
                ? MVVMConstants.NONE
                : ViewModelsEditorUtility.ToShortName(viewModelTypeFullName.stringValue, _cachedViewModelTypes);
            var viewModelGuiContent = new GUIContent(buttonDisplayName);
            
            if (EditorGUI.DropdownButton(viewModelTypeButtonRect, viewModelGuiContent, FocusType.Keyboard))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), provider);
            }
            
            EditorGUI.PropertyField(prefabViewRect, prefabView, _prefabViewGUIContent);
            
            EditorGUI.indentLevel -= 1;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3 + 10;
        }
    }
}