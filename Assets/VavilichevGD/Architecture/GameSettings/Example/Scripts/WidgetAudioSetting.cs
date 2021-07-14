using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.Architecture.Settings.Example {
	public abstract class WidgetAudioSetting : WidgetSetting {

		[SerializeField] private Button buttonUp;
		[SerializeField] private Button buttonDown;
		[SerializeField] protected Text textVolume;


		private void OnEnable() {
			buttonUp.onClick.AddListener(OnButtonUpClicked);
			buttonDown.onClick.AddListener(OnButtonDownClicked);
		}

		private void OnDisable() {
			buttonUp.onClick.RemoveListener(OnButtonUpClicked);
			buttonDown.onClick.RemoveListener(OnButtonDownClicked);
		}


		#region CALLBACKS

		protected abstract void OnButtonUpClicked();
		protected abstract void OnButtonDownClicked();

		#endregion
	}
}