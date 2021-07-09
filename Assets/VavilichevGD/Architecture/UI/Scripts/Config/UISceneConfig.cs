using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VavilichevGD.Utils.Attributes;

namespace VavilichevGD.Architecture.UserInterface {
	[CreateAssetMenu(fileName = "UIPrefabsOnSceneConfig", menuName = "Architecture/UI/New UIPrefabsOnSceneConfig")]
	public class UISceneConfig : ScriptableObject {

		[SerializeField, SceneName] private string _sceneName;

		[SerializeField] 
		[GameObjectOfType(typeof(IUIElementOnLayer))]
		private List<GameObject> _prefabs;


		public string sceneName => _sceneName;
		public IUIElementOnLayer[] prefabs => this.GetPrefabs();

		public IUIElementOnLayer[] GetPrefabs() {
			var uiPrefabs = new List<IUIElementOnLayer>();
			foreach (var goPrefab in _prefabs) {
				var uiPrefab = goPrefab.GetComponent<IUIElementOnLayer>();
				uiPrefabs.Add(uiPrefab);
			}

			return uiPrefabs.ToArray();
		}

		public IUIElementOnLayer GetPrefab(Type type) {
			var allPrefab = prefabs;
			return allPrefab.First(pref => pref.GetType() == type);
		}
	}
}