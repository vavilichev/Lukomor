using System;
using System.Collections.Generic;
using System.Linq;
using Lukomor.MVVM.Attributes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    [CustomPropertyDrawer(typeof(ViewModelPropertyAttribute))]
    public class ViewModelPropertyDrawer : PropertyDrawer
    {
        private readonly List<string> _options = new();
        private readonly GUIStyle _labelStyle = new(EditorStyles.label) {fixedWidth = 205f};

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.HelpBox(position,$"Cannot show field \"{property.name}\". Use ViewModelProperty attribute only with string field", MessageType.Warning);
                return;
            }

            var binder = (PropertyBinder)property.serializedObject.targetObject;
            var binderGO = binder.gameObject;
            var parentView = binderGO.GetComponentInParent<IView>(true);
            var viewModelType = parentView.ViewModelType;
            var binderArgumentType = binder.GetGenericArgumentType();

            position.y += 5f;
            
            UpdateOptionsList(viewModelType, binderArgumentType);
            CleanupFieldIfRequired(_options, property);
            
            EditorGUILayout.BeginHorizontal();
            
            EditorGUI.LabelField(position, "Property:", _labelStyle);
            position.x += _labelStyle.fixedWidth;
            position.width -= _labelStyle.fixedWidth;
            
            if (GUI.Button(position, property.stringValue, EditorStyles.popup))
            {
                var provider = ScriptableObject.CreateInstance<StringListSearchProvider>();
                provider.Init(_options.ToArray(), value =>
                {
                    property.stringValue = value;
                    property.serializedObject.ApplyModifiedProperties();
                });
                
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), provider);
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
        }
        
        private void UpdateOptionsList(Type viewModelType, Type propertyType)
        {
            _options.Clear();

            var allProperties = viewModelType.GetProperties();
            var requiredProperties = allProperties.Where(p =>
            {
                if (propertyType != null)
                {
                    var type = p.PropertyType;
                    try
                    {
                        if (!type.IsGenericType)
                        {
                            return false;
                        }
                        
                        var vmPropertyArgumentType = type.GetGenericArguments().Last();
                        var isValidPropertyType = propertyType.IsAssignableFrom(vmPropertyArgumentType);
                    
                        return p.CanRead && isValidPropertyType;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
                
                return p.CanRead;
            });
            
            _options.AddRange(requiredProperties.Select(p => p.Name));
        }

        private static void CleanupFieldIfRequired(List<string> updatedOptions, SerializedProperty property)
        {
            if (!updatedOptions.Contains(property.stringValue))
            {
                property.stringValue = null;
                
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}