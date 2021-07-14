using UnityEngine;

namespace VavilichevGD.Architecture.Settings.Example {
	public sealed class WidgetSettingMusic : WidgetAudioSetting {
		protected override void OnToggleValueChanged(bool isOn) {
			settings.audioSettings.isMusicEnabled = isOn;
			settings.Save();
		}

		protected override void OnButtonUpClicked() {
			var step = 0.1f;
			settings.audioSettings.volumeMusic += step;
			settings.Save();
			UpdateVisual();
		}

		protected override void OnButtonDownClicked() {
			var step = 0.1f;
			settings.audioSettings.volumeMusic -= step;
			settings.Save();
			UpdateVisual();
		}

		protected override void UpdateVisual() {
			var intValue = Mathf.RoundToInt(settings.audioSettings.volumeMusic * 10);
			textVolume.text = intValue.ToString();
			toggle.isOn = settings.audioSettings.isMusicEnabled;
		}
	}
}