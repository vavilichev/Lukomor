using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace VavilichevGD.Utils.Attributes {
	[CustomPropertyDrawer(typeof(SceneNameAttribute))]
	public class SceneNameAttributeDrawer : PropertyDrawer {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			this.DrawPrefix(position, property);
			this.DrawPopup(position, property);
			this.DrawWarning(position);
			this.DrawButtonForAddingScene(position);
		}

		private void DrawPrefix(Rect position, SerializedProperty property) {
			EditorGUI.PrefixLabel(position, new GUIContent(property.displayName));
		}

		private void DrawPopup(Rect position, SerializedProperty property) {
			position.x += Screen.width / 4;
			position.width -= Screen.width / 4;
			position.height = EditorGUIUtility.singleLineHeight;
			
			var sceneNames = new List<string>();
			foreach (var scene in EditorBuildSettings.scenes) {
				var sceneName = Path.GetFileNameWithoutExtension(scene.path);
				sceneNames.Add(sceneName);
			}
			
			if (sceneNames.Count == 0)
				return;
			
			var sceneNameSelected = property.stringValue;
			var classNameSelectedIndex = sceneNames.IndexOf(sceneNameSelected);
			classNameSelectedIndex = Mathf.Clamp(classNameSelectedIndex, 0, sceneNames.Count - 1);
			classNameSelectedIndex = EditorGUI.Popup(position, classNameSelectedIndex, sceneNames.ToArray());
			
			property.stringValue = sceneNames[classNameSelectedIndex];
		}

		private void DrawWarning(Rect position) {
			position.width -= Screen.width / 5 * 2;
			position.x += Screen.width / 4;
			position.y += EditorGUIUtility.singleLineHeight + 5;
			position.height = EditorGUIUtility.singleLineHeight * 2;
			EditorGUI.HelpBox(position, "Don't forget to add scene to build settings. Otherwise you cannot chose it in the scene name list.", MessageType.Warning);
		}

		private void DrawButtonForAddingScene(Rect position) {
			position.width -= Screen.width / 6 * 5;
			position.x += Screen.width / 6 * 5;
			position.y += EditorGUIUtility.singleLineHeight + 5;
			position.height = EditorGUIUtility.singleLineHeight * 2;
			if (GUI.Button(position, "Add current")) {
				var currentScenePath = EditorSceneManager.GetActiveScene().path;
				var buildScenes = EditorBuildSettings.scenes;
			
				if (buildScenes.Any(item => item.path == currentScenePath))
					return;
			
				var editorBuildSettingsScenes = new List<EditorBuildSettingsScene>(buildScenes);
				editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(currentScenePath, true));
				EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
			}
		}
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight * 2 + 5;
		}

	}
}