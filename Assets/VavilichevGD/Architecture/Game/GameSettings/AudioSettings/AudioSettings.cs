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
			get => this.isSFXEnabled && this.isMusicEnabled;
			set {
				this.isSFXEnabled = value;
				this.isMusicEnabled = value;
				this.Save();
			}
		}

		public bool isSFXEnabled {
			get => this.m_isSFXEnabled;
			set {
				this.m_isSFXEnabled = value;
				this.OnVolumeSFXChangedEvent?.Invoke();
			}
		}

		public bool isMusicEnabled {
			get => this.m_isMusicEnabled;
			set {
				this.m_isMusicEnabled = value;
				this.OnVolumeMusicChangedEvent?.Invoke();
			}
		}

		
		public float volumeSFX {
			get => this.m_volumeSFX;
			set {
				this.m_volumeSFX = value;
				this.OnVolumeSFXChangedEvent?.Invoke();
			}
		}
		
		public float volumeMusic {
			get => this.m_volumeMusic;
			set {
				this.m_volumeMusic = value;
				this.OnVolumeMusicChangedEvent?.Invoke();
			}
		}


		private bool m_isSFXEnabled;
		private bool m_isMusicEnabled;

		private float m_volumeSFX;
		private float m_volumeMusic;
		

		public AudioSettings() {
			var propertiesDefault = new AudioSettingsProperties();
			this.m_isSFXEnabled = propertiesDefault.isSFXEnabled;
			this.m_isMusicEnabled = propertiesDefault.isMusicEnabled;
			this.m_volumeSFX = propertiesDefault.volumeSFX;
			this.m_volumeMusic = propertiesDefault.volumeMusic;
		}
		
		public void Load() {
			var propsDefault = new AudioSettingsProperties();
			var propsDefaultJson = propsDefault.ToJson();
			var propsLoadedJson = PlayerPrefs.GetString(KEY_AUDIO_SETTING, propsDefaultJson);
			var loadedProperties = JsonUtility.FromJson<AudioSettingsProperties>(propsLoadedJson);

			this.isSFXEnabled = loadedProperties.isSFXEnabled;
			this.volumeSFX = loadedProperties.volumeSFX;
			this.OnVolumeSFXChangedEvent?.Invoke();

			this.isMusicEnabled = loadedProperties.isMusicEnabled;
			this.volumeMusic = loadedProperties.volumeMusic;
		}

		public void Save() {
			var props = new AudioSettingsProperties(this);
			var propsJson = props.ToJson();
			PlayerPrefs.SetString(KEY_AUDIO_SETTING, propsJson);
		}

		public override string ToString() {
			var line =
				$"SFX: is enabled = {this.isSFXEnabled}, volume = {this.volumeSFX}\n" +
				$"Music: is enabled = {this.isMusicEnabled}, volume = {this.volumeMusic}";
			return line;
		}
	}
}