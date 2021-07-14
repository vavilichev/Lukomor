using System;
using VavilichevGD.Architecture.StorageSystem;

namespace VavilichevGD.Architecture.Settings {
	public class VibroSettings : IVibroSettings {

		#region CONSTANTS

		private const string KEY_VIBRO_SETTINGS = "VIBRO_SETTINGS";

		#endregion

		#region EVENTS

		public event Action OnVibroStateChangedEvent;

		#endregion

		public bool isEnabled {
			get => this.vibroData.isVibroEnabled;
			set {
				vibroData.isVibroEnabled = value;
				this.OnVibroStateChangedEvent?.Invoke();
			}
		}

		private Storage gameSettingsStorage;
		private VibroSettingsData vibroData;

		public VibroSettings(Storage gameSettingsStorage) {
			this.gameSettingsStorage = gameSettingsStorage;
			var vibroDataDefault = new VibroSettingsData();
			var loadedData = gameSettingsStorage.Get(KEY_VIBRO_SETTINGS, vibroDataDefault);
			vibroData = loadedData;
		}
		

		public void Save() {
			gameSettingsStorage.Set(KEY_VIBRO_SETTINGS, vibroData);
		}

		public override string ToString() {
			var line = $"VIBRO: is enabled = {this.isEnabled}";
			return line;
		}
	}
}