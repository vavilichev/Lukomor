using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture.UserInterface.Utils;

namespace VavilichevGD.Architecture.UserInterface.Example {
	public class UIScreenGameExample : UIScreen {

		[SerializeField] private Button buttonExit;
		
		private void OnEnable() {
			this.buttonExit.AddListener(this.OnExamplePopupButtonClick);
		}

		private void OnDisable() {
			this.buttonExit.RemoveListener(this.OnExamplePopupButtonClick);
		}

		#region EVENTS

		private void OnExamplePopupButtonClick() {
			this.uiCOntroller.ShowUIElement<UIPopupExitExample>();
		}

		#endregion
	}
}