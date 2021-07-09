using UnityEngine.Events;
using UnityEngine.UI;

namespace VavilichevGD.Architecture.UserInterface.Utils {
	public static class UIExtensions {

		public static void AddListener(this Button button, UnityAction callback) {
			button.onClick.AddListener(callback);
		}

		public static void RemoveListener(this Button button, UnityAction callback) {
			button.onClick.RemoveListener(callback);
		}

		public static void RemoveAllListeners(this Button button) {
			button.onClick.RemoveAllListeners();
		}
	}
}