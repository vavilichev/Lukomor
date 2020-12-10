using System;

namespace VavilichevGD.Architecture.UI {
	public interface IUIElement {

		#region EVENTS

		event Action<IUIElement> OnElementHideStartedEvent;
		event Action<IUIElement> OnElementHiddenCompletelyEvent;
		event Action<IUIElement> OnElementShownEvent;

		#endregion
		
		bool isActive { get; }
		
		void Show();
		void Hide();
		void HideInstantly();
		
	}
}