using System;
using UnityEngine;
using UnityEditor;
using VavilichevGD.Architecture.UserInterface.Extensions;

namespace VavilichevGD.Utils.Attributes {
	[CustomPropertyDrawer(typeof(GameObjectOfTypeAttribute))]
	public class GameObjectOfTypeDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			if (property.propertyType != SerializedPropertyType.ObjectReference)
				throw new Exception(
					$"You can use only GameObject fields with GameObjectOfType attribute. Class: {property.serializedObject.targetObject.GetType()}");
			
			position.height = EditorGUIUtility.singleLineHeight;

			var myAttribute = attribute as GameObjectOfTypeAttribute;
			var isArray = fieldInfo.FieldType.IsArrayOrList();
			var labelName = label.text + $" ({myAttribute.type.Name})";
			var currentEvent = Event.current;
			var onHovered = position.Contains(currentEvent.mousePosition);

			if (isArray) {
				var partsOfName = label.text.Split(' ');
				labelName = $"{myAttribute.type.Name} {Convert.ToInt16(partsOfName[1])}";
			}

			if (DragAndDrop.objectReferences.Length > 0 && onHovered) {
				foreach (var o in DragAndDrop.objectReferences) {
					if (o is GameObject gameObject && gameObject.GetComponent(myAttribute.type))
						continue;
					DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
				}
			}
			
			if (property.objectReferenceValue != null) {
				if (!(property.objectReferenceValue is GameObject go) || go.GetComponent(myAttribute.type) == null)
					throw new Exception($"You must add only {myAttribute.type} objects.");
			}

			property.objectReferenceValue =
				EditorGUI.ObjectField(position, labelName, property.objectReferenceValue, typeof(GameObject), true);
		}
	}
}