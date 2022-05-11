using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VavilichevGD.Utils.Attributes.ObjectsOfType {
	public abstract class ObjectOfTypeDrawerBase : PropertyDrawer{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			var objectType = GetCurrentObjectType();
			
			if (objectType == null) {
				DrawErrorOfType(position);
				return;
			}
			
			var ootAttribute = attribute as ObjectOfTypeAttributeBase;
			var requiredType = ootAttribute.type;

			CheckDragAndDrops(position, requiredType);
			CheckValues(property, requiredType);
			DrawObjectField(position, label, property, requiredType, objectType, ootAttribute.allowSceneObjects);
		}
		
		protected abstract Type GetCurrentObjectType();
		protected abstract Type GetRequiredObjectType();
		protected abstract bool IsValidObject(Object o, Type requiredType);

		protected bool HasObjectType<T>() {
			return fieldInfo.FieldType == typeof(T) || typeof(IEnumerable<T>).IsAssignableFrom(fieldInfo.FieldType);
		}

		private void CheckDragAndDrops(Rect position, Type requiredType) {
			if (position.Contains(Event.current.mousePosition)) {
				var draggedObjectsCount = DragAndDrop.objectReferences.Length;
				
				for (int i = 0; i < draggedObjectsCount; i++) {
					if (!IsValidObject(DragAndDrop.objectReferences[i], requiredType)) {
						DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
						break;
					}
				}
			}
		}

		private void CheckValues(SerializedProperty property, Type requiredType) {
			if (property.objectReferenceValue != null) {
				if (!IsValidObject(property.objectReferenceValue, requiredType)) {
					property.objectReferenceValue = null;
				}
			}
		}

		private void DrawObjectField(
			Rect position,
			GUIContent label,
			SerializedProperty property,
			Type requiredType,
			Type objectType,
			bool allowSceneObjects) {
			
			label.text =  $"{label.text} ({requiredType.Name})";;

			property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, objectType, allowSceneObjects);
		}

		private void DrawErrorOfType(Rect position) {
			var requiredObjectType = GetRequiredObjectType();
			
			EditorGUI.HelpBox(position, $"{attribute.GetType().Name} works only with {requiredObjectType.Name} references", MessageType.Error);
		}
	}
}