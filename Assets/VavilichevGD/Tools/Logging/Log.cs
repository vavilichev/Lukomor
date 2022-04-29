using UnityEngine;

namespace VavilichevGD.Tools.Logging {
	public static class Log {
		public static void Print(string text) {
			Debug.Log(text);
		}

		public static void Print(string text, GameObject gameObject) {
			Debug.Log(text, gameObject);
		}

		public static void PrintWarning(string text) {
			Debug.LogWarning(text);
		}

		public static void PrintWarning(string text, GameObject gameObject) {
			Debug.LogWarning(text, gameObject);
		}

		public static void PrintError(string text) {
			Debug.LogError(text);
		}

		public static void PrintError(string text, GameObject gameObject) {
			Debug.LogError(text, gameObject);
		}

		public static void CheckForNull<T>(T o, string errorMessage) {
			if (o == null) {
				Debug.Log(errorMessage);
			}
		}
	}
}