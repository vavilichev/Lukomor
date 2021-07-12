using System;
using UnityEngine;

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
				Save();
			}
		}

		public bool isSFXEnabled {
			get => _isSFXEnabled;
			set {
				_isSFXEnabled = value;
				OnVolumeSFXChangedEvent?.Invoke();
			}
		}

		public bool isMusicEnabled {
			get => _isMusicEnabled;
			set {
				_isMusicEnabled = value;
				OnVolumeMusicChangedEvent?.Invoke();
			}
		}

		
		public float volumeSFX {
			get => _volumeSFX;
			set {
				_volumeSFX = value;
				OnVolumeSFXChangedEvent?.Invoke();
			}
		}
		
		public float volumeMusic {
			get => _volumeMusic;
			set {
				_volumeMusic = value;
				OnVolumeMusicChangedEvent?.Invoke();
			}
		}


		private bool _isSFXEnabled;
		private bool _isMusicEnabled;

		private float _volumeSFX;
		private float _volumeMusic;
		

		public AudioSettings() {
			var propertiesDefault = new AudioSettingsProperties();
			_isSFXEnabled = propertiesDefault.isSFXEnabled;
			_isMusicEnabled = propertiesDefault.isMusicEnabled;
			_volumeSFX = propertiesDefault.volumeSFX;
			_volumeMusic = propertiesDefault.volumeMusic;
		}
		
		public void Load() {
			var propsDefault = new AudioSettingsProperties();
			var propsDefaultJson = propsDefault.ToJson();
			var propsLoadedJson = PlayerPrefs.GetString(KEY_AUDIO_SETTING, propsDefaultJson);
			var loadedProperties = JsonUtility.FromJson<AudioSettingsProperties>(propsLoadedJson);

			isSFXEnabled = loadedProperties.isSFXEnabled;
			volumeSFX = loadedProperties.volumeSFX;
			OnVolumeSFXChangedEvent?.Invoke();

			isMusicEnabled = loadedProperties.isMusicEnabled;
			volumeMusic = loadedProperties.volumeMusic;
		}

		public void Save() {
			var props = new AudioSettingsProperties(this);
			var propsJson = props.ToJson();
			PlayerPrefs.SetString(KEY_AUDIO_SETTING, propsJson);
		}

		public override string ToString() {
			var line =
				$"SFX: is enabled = {isSFXEnabled}, volume = {volumeSFX}\n" +
				$"Music: is enabled = {isMusicEnabled}, volume = {volumeMusic}";
			return line;
		}
	}
}