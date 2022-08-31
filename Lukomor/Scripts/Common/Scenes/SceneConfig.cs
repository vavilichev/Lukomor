using System.Collections.Generic;
using Lukomor.Presentation.Views.Windows;
using UnityEngine;
using VavilichevGD.Utils.Attributes.ObjectsOfType;

namespace Lukomor.Common.Scenes
{
	[CreateAssetMenu(fileName = "SceneConfig", menuName = "Application/Scenes/New Scene Config")]
	public class SceneConfig : ScriptableObject, ISceneConfig
	{
		[SerializeField] private string _sceneName;

		[SerializeField]
		private List<GameObject> _sceneWindowPrefabs;

		public string SceneName => _sceneName;
		public IWindow[] WindowPrefabs => GetWindowPrefabs();

		private IWindow[] GetWindowPrefabs()
		{
			var prefabs = new IWindow[_sceneWindowPrefabs.Count];
			var prefabsCount = prefabs.Length;

			for (int i = 0; i < prefabsCount; i++)
			{
				prefabs[i] = _sceneWindowPrefabs[i].GetComponent<IWindow>();
			}

			return prefabs;
		}
	}
}
