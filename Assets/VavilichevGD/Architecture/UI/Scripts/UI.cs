using System.Linq;
using UnityEngine;

namespace VavilichevGD.Architecture.UserInterface {
	public static class UI {

		#region CONSTANTS

		private const string PATH_UI_CONTROLLER_PREFAB = "[INTERFACE]";
		private const string PATH_UI_CONFIG_FOLDER = "SceneConfigs";

		#endregion

		public static UIController controller { get; private set; }


		public static void Build(string sceneName) {
			if (controller == null)
				controller = CreateUIController();

			controller.Clear();

			var allUISceneConfigs = Resources.LoadAll<UISceneConfig>(PATH_UI_CONFIG_FOLDER);
			var sceneConfig = allUISceneConfigs.First(config => config.sceneName == sceneName);
			controller.BuildUI(sceneConfig);
		}

		private static UIController CreateUIController() {
			var pref = Resources.Load<UIController>(PATH_UI_CONTROLLER_PREFAB);
			var createdController = Object.Instantiate(pref);
			createdController.name = pref.name;
			Resources.UnloadUnusedAssets();
			return createdController;
		}

	}
}