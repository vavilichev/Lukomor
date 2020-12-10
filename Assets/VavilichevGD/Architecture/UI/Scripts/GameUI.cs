using UnityEngine;

namespace VavilichevGD.Architecture.UI {
	public static class GameUI {

		#region CONSTANTS

		private const string PATH_UI_CONTROLLER_PREFAB = "[INTERFACE]";

		#endregion

		public static IUIController CreateUIContollerInstance() {
			var pref = Resources.Load<UIController>(PATH_UI_CONTROLLER_PREFAB);
			var createdUIController = Object.Instantiate(pref);
			createdUIController.name = pref.name;
			return createdUIController;
		}
		
	}
}