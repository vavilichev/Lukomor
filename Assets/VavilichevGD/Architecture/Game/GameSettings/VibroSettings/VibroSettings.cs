using System;
using UnityEngine;

namespace VavilichevGD.Architecture.Settings {
	public class VibroSettings : IVibroSettings {

		#region CONSTANTS

		private const string KEY_VIBRO_SETTINGS = "VIBRO_SETTINGS";

		#endregion

		#region EVENTS

		public event Action OnVibroStateChangedEvent;

		#endregion

		public bool isEnabled {
			get => this.m_isEnabled;
			set {
				this.m_isEnabled = value;
				this.OnVibroStateChangedEvent?.Invoke();
			}
		}

		private bool m_isEnabled;


		public VibroSettings() {
			this.m_isEnabled = true;
		}
		

		public void Load() {
			var intDefaultValue = this.isEnabled ? 1 : 0;
			var intLoadedValue = PlayerPrefs.GetInt(KEY_VIBRO_SETTINGS, intDefaultValue);
			var loadedValue = intLoadedValue == 1;
			this.isEnabled = loadedValue;
		}
		

		public void Save() {
			var intValue = this.isEnabled ? 1 : 0;
			PlayerPrefs.SetInt(KEY_VIBRO_SETTINGS, intValue);
		}

		public override string ToString() {
			var line = $"VIBRO: is enabled = {this.isEnabled}";
			return line;
		}
	}
}