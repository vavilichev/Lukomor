using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture.UserInterface.Utils;

namespace VavilichevGD.Architecture.UserInterface.Example {
	public class UIScreenGameExample : UIScreen {

		[SerializeField] private Button buttonExit;
		[SerializeField] private UIWidgetInfoGameExample widget;
		
		private void OnEnable() {
			buttonExit.AddListener(OnExamplePopupButtonClick);
		}

		private void OnDisable() {
			buttonExit.RemoveListener(OnExamplePopupButtonClick);
		}

		#region EVENTS

		private void OnExamplePopupButtonClick() {
			uiController.ShowUIElement<UIPopupExitExample>();
		}

		public override void OnStart() {
			widget.OnStart();
		}

		#endregion
	}
}