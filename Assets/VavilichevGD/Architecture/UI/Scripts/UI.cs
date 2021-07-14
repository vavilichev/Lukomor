using UnityEngine;

namespace VavilichevGD.Architecture.UserInterface {
	public static class UI {

		#region CONSTANTS

		private const string PATH_UI_CONTROLLER_PREFAB = "[INTERFACE]";

		#endregion

		public static UIController controller { get; private set; }


		public static void Build(SceneConfig sceneConfig) {
			if (controller == null)
				controller = CreateUIController();

			controller.Clear();
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