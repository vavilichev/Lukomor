using System;
using UnityEngine;
using VavilichevGD.Architecture.StorageSystem;

namespace VavilichevGD.Architecture.Settings {
	public class AudioSettings : IAudioSettings {

		#region CONSTANTS

		protected const string KEY_AUDIO_SETTING = "AUDIO_SETTINGS";

		#endregion

		#region EVENTS

		public event Action OnVolumeSFXChangedEvent;
		public event Action OnVolumeMusicChangedEvent;

		#endregion

		
		public bool isEnabled {
			get => isSFXEnabled && isMusicEnabled;
			set {
				isSFXEnabled = value;
				isMusicEnabled = value;
			}
		}

		public bool isSFXEnabled {
			get => audioData.isSFXEnabled;
			set {
				audioData.isSFXEnabled = value;
				OnVolumeSFXChangedEvent?.Invoke();
			}
		}

		public bool isMusicEnabled {
			get => audioData.isMusicEnabled;
			set {
				audioData.isMusicEnabled = value;
				OnVolumeMusicChangedEvent?.Invoke();
			}
		}

		
		public float volumeSFX {
			get => audioData.volumeSFX;
			set {
				audioData.volumeSFX = Mathf.Clamp(value, 0f, 1f);
				OnVolumeSFXChangedEvent?.Invoke();
			}
		}
		
		public float volumeMusic {
			get => audioData.volumeMusic;
			set {
				audioData.volumeMusic = Mathf.Clamp(value, 0f, 1f);
				OnVolumeMusicChangedEvent?.Invoke();
			}
		}


		private Storage gameSettingsStorage;
		private AudioSettingsData audioData;
		

		public AudioSettings(Storage gameSettingsStorage) {
			this.gameSettingsStorage = gameSettingsStorage;
			var audioDataByDefault = new AudioSettingsData();
			var loadedData = gameSettingsStorage.Get(KEY_AUDIO_SETTING, audioDataByDefault);
			audioData = loadedData;
		}
		
		public void Save() {
			gameSettingsStorage.Set(KEY_AUDIO_SETTING, audioData);
			gameSettingsStorage.Save();
		}

		public override string ToString() {
			var line =
				$"SFX: is enabled = {isSFXEnabled}, volume = {volumeSFX}\n" +
				$"Music: is enabled = {isMusicEnabled}, volume = {volumeMusic}";
			return line;
		}
	}
}