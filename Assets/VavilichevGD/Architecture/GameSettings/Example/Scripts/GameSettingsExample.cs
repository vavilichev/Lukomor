using UnityEngine;

namespace VavilichevGD.Architecture.Settings.Example {
	public sealed class GameSettingsExample : MonoBehaviour {

		public GameSettings settings { get; private set; }

		private void Awake() {
			settings = new GameSettings(true);
		}

	}
}