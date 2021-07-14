using UnityEngine;

namespace VavilichevGD.Architecture.Settings.Example {
	public sealed class WidgetSettingSFX : WidgetAudioSetting {

		protected override void OnToggleValueChanged(bool isOn) {
			settings.audioSettings.isSFXEnabled = isOn;
			settings.Save();
		}

		protected override void OnButtonUpClicked() {
			var step = 0.1f;
			settings.audioSettings.volumeSFX += step;
			settings.Save();
			UpdateVisual();
		}

		protected override void OnButtonDownClicked() {
			var step = 0.1f;
			settings.audioSettings.volumeSFX -= step;
			settings.Save();
			UpdateVisual();
		}

		protected override void UpdateVisual() {
			var intValue = Mathf.RoundToInt(settings.audioSettings.volumeSFX * 10);
			textVolume.text = intValue.ToString();
			toggle.isOn = settings.audioSettings.isSFXEnabled;
		}
	}
}