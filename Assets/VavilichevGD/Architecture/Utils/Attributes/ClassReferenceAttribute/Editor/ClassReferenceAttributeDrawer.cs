using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace VavilichevGD.Utils.Attributes {
	[CustomPropertyDrawer(typeof(ClassReferenceAttribute))]
	public class ClassReferenceAttributeDrawer : PropertyDrawer {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.PrefixLabel(position, new GUIContent(property.displayName));
			position.x += Screen.width / 4;
			position.width -= Screen.width / 4;


			var classNameSelected = property.stringValue;
			var classAttribute = attribute as ClassReferenceAttribute;
			var type = classAttribute.type;
			
			var typesMap = this.GetInheritedTypesMap(type);
			var typeNames = typesMap.Keys.ToList();
			var typeFullNames = typesMap.Values.ToList();
			var classNameSelectedIndex = typeFullNames.IndexOf(classNameSelected);
			
			classNameSelectedIndex = Mathf.Clamp(classNameSelectedIndex, 0, typesMap.Count - 1);
			classNameSelectedIndex = EditorGUI.Popup(position, classNameSelectedIndex, typeNames.ToArray());

			property.stringValue = typeFullNames[classNameSelectedIndex];
		}

		private Dictionary<string, string> GetInheritedTypesMap(Type baseType) {
			var sortedObjects = new SortedDictionary<string, string>();
			foreach (var type in Assembly.GetAssembly(baseType).GetTypes().Where
					(myType => myType.IsClass && myType.IsSubclassOf(baseType))) {
				sortedObjects[type.Name] = type.FullName;
			}
			
			var objects = new Dictionary<string, string>();
			foreach (var item in sortedObjects) 
				objects[item.Key] = item.Value;

			return objects;
		}
	}
}