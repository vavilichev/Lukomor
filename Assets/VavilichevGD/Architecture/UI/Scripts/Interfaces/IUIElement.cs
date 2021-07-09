using System;
using UnityEngine;

namespace VavilichevGD.Architecture.UserInterface {
	public interface IUIElement {

		#region EVENTS

		event Action<IUIElement> OnElementHideStartedEvent;
		event Action<IUIElement> OnElementHiddenCompletelyEvent;
		event Action<IUIElement> OnElementShownEvent;
		event Action<IUIElement> OnElementDestroyedEvent;

		#endregion

		bool isActive { get; }
		string name { get; }
		GameObject gameObject { get; }

		void Show();
		void Hide();
		void HideInstantly();

	}
}