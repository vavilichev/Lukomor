using System;

namespace VavilichevGD.Architecture.Settings {
	[Serializable]
	public class AudioSettingsData {

		#region CONSTANTS

		public const float MAX_VOLUME = 1f;
		public const float MIN_VOLUME = 0f;

		#endregion

		public bool isSFXEnabled;
		public bool isMusicEnabled;
		public float volumeSFX;
		public float volumeMusic;

		public AudioSettingsData() {
			this.isSFXEnabled = true;
			this.isMusicEnabled = true;
			this.volumeSFX = MAX_VOLUME;
			this.volumeMusic = MAX_VOLUME;
		}

		public AudioSettingsData(IAudioSettings audioSettings) {
			this.isSFXEnabled = audioSettings.isSFXEnabled;
			this.isMusicEnabled = audioSettings.isMusicEnabled;
			this.volumeSFX = audioSettings.volumeSFX;
			this.volumeMusic = audioSettings.volumeMusic;
		}

	}
}