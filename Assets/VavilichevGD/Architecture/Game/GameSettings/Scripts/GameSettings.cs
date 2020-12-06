using VavilichevGD.Tools;

namespace VavilichevGD.Architecture.Settings {
	public class GameSettings : IGameSettings {
		
		public IAudioSettings audioSettings { get; }
		public IVibroSettings vibroSettings { get; }

		public GameSettings() {
			this.audioSettings = new AudioSettings();
			this.vibroSettings = new VibroSettings();
		}
		
		public void Load() {
			this.audioSettings.Load();
			this.vibroSettings.Load();
			
			Logging.Log($"GAME SETTINGS LOADED.\n{this.audioSettings}\n{this.vibroSettings}");
		}

		public void Save() {
			this.audioSettings.Save();
			this.vibroSettings.Save();
		}

	}
}