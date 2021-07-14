using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.Architecture.Settings.Example {
	public abstract class WidgetSetting : MonoBehaviour {

		[SerializeField] protected GameSettingsExample example;
		[SerializeField] protected Toggle toggle;


		protected GameSettings settings => example.settings;

		private IEnumerator Start() {
			toggle.onValueChanged.AddListener(OnToggleValueChanged);

			while (example.settings == null)
				yield return null;


			UpdateVisual();
		}

		private void OnDestroy() {
			toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
		}

		protected abstract void OnToggleValueChanged(bool isOn);
		protected virtual void UpdateVisual() { }

	}
}