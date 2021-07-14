using UnityEngine;

namespace VavilichevGD.Architecture.Settings.Example {
	public sealed class WidgetSettingVibro : WidgetSetting {
		protected override void OnToggleValueChanged(bool isOn) {
			settings.vibroSettings.isEnabled = isOn;
			settings.Save();
		}

		protected override void UpdateVisual() {
			toggle.isOn = settings.vibroSettings.isEnabled;
		}
	}
}