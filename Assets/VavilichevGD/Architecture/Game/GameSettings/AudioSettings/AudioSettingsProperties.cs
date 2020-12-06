using System;
using UnityEngine;

namespace VavilichevGD.Architecture.Settings {
	[Serializable]
	public class AudioSettingsProperties {

		#region CONSTANTS

		public const float MAX_VOLUME = 1f;
		public const float MIN_VOLUME = 0f;

		#endregion

		public bool isSFXEnabled;
		public bool isMusicEnabled;
		public float volumeSFX;
		public float volumeMusic;

		public AudioSettingsProperties() {
			this.isSFXEnabled = true;
			this.isMusicEnabled = true;
			this.volumeSFX = MAX_VOLUME;
			this.volumeMusic = MAX_VOLUME;
		}

		public AudioSettingsProperties(IAudioSettings audioSettings) {
			this.isSFXEnabled = audioSettings.isSFXEnabled;
			this.isMusicEnabled = audioSettings.isMusicEnabled;
			this.volumeSFX = audioSettings.volumeSFX;
			this.volumeMusic = audioSettings.volumeMusic;
		}

		public string ToJson() {
			return JsonUtility.ToJson(this);
		}

	}
}