using UnityEngine;
using VavilichevGD.Architecture.StorageSystem;

namespace VavilichevGD.Architecture.Settings {
	public sealed class GameSettings : IGameSettings {

		#region CONSTANTS

		private const string SETTINGS_FILE_NAME = "GameSettings.data"; 

		#endregion
		
		public IAudioSettings audioSettings { get; }
		public IVibroSettings vibroSettings { get; }
		public bool isLoggingEnabled { get; set; }

		private Storage settingsStorage;

		public GameSettings(bool isLoggingEnabled = false) {
			this.isLoggingEnabled = isLoggingEnabled;
			
			settingsStorage = new FileStorage(SETTINGS_FILE_NAME);
			settingsStorage.Load();
			
			audioSettings = new AudioSettings(settingsStorage);
			vibroSettings = new VibroSettings(settingsStorage);

			if (isLoggingEnabled) 
				Debug.Log($"GAME SETTINGS LOADED: \n{audioSettings}\n{vibroSettings}");
		}
		

		public void Save() {
			this.audioSettings.Save();
			this.vibroSettings.Save();
			
			if (isLoggingEnabled) 
				Debug.Log($"GAME SETTINGS SAVED: \n{audioSettings}\n{vibroSettings}");
		}

	}
}