using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using VavilichevGD.Architecture;

public class InteractorNameAttribute : PropertyAttribute {

	public static readonly int INITIAL_SIZE = 1;
	public string[] values;

	public InteractorNameAttribute() {
		values = new string[INITIAL_SIZE];
		for (int i = 0; i < INITIAL_SIZE; i++)
			values[i] = "";
	}

}

[CustomPropertyDrawer(typeof(InteractorNameAttribute))]
public class InteractorNameDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		
		
		EditorList.Show(property, EditorList.EditorListOption.All);
		
		/*
		
		
		
		Debug.Log($"Is array: {label.text}");
		
		
		var list = GetEnumerableOfType<Interactor>();
		var sourceProperty = property.serializedObject.FindProperty(property.name);
		
		EditorGUI.PrefixLabel(position, new GUIContent(property.name));
		position.x += Screen.width / 4;
		position.width -= Screen.width / 4;
		
		
		var classNameSelected = sourceProperty.stringValue;
		var classNameSelectedIndex = list.IndexOf(classNameSelected);
		classNameSelectedIndex = Mathf.Clamp(classNameSelectedIndex, 0, list.Count - 1);
		classNameSelectedIndex = EditorGUI.Popup(position, classNameSelectedIndex, list.ToArray());
		
		sourceProperty.stringValue = list[classNameSelectedIndex];
		
		//
		// if (sourceProperty.isArray) {
		// 	
		// 	SerializedProperty arrayProp = property.FindPropertyRelative("values");
		// 	for (int i = 0; i < arrayProp.arraySize; i++)
		// 	{
		// 		// This will display an Inspector Field for each array item (layout this as desired)
		// 		SerializedProperty value = arrayProp.GetArrayElementAtIndex(i);
		// 		EditorGUI.PropertyField(position, value, GUIContent.none);
		// 	}
		// }
		// else {
		// 	
		// }
		
		
		
		*/

		
	}
	
		
		
		
		
		
	// public override void ArrayGUI (SerializedProperty property) {
	// 	SerializedProperty arraySizeProp = property.FindPropertyRelative("Array.size");
	// 	EditorGUILayout.PropertyField(arraySizeProp);
 //
	// 	EditorGUI.indentLevel ++;
 //
	// 	for (int i = 0; i < arraySizeProp.intValue; i++) {
	// 		EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i));
	// 	}
 //
	// 	EditorGUI.indentLevel --;
	// }

	public static List<string> GetEnumerableOfType<T>() where T : Interactor
	{
		var objects = new List<string>();
		foreach (Type type in 
			Assembly.GetAssembly(typeof(T)).GetTypes()
				.Where(myType => myType.IsClass && myType.IsSubclassOf(typeof(T))))
		{
			objects.Add(type.Name);
		}
		objects.Sort();
		return objects;
	}
	
}