using System;

namespace VavilichevGD.Architecture.UI {
	public interface IUIController {

		#region EVENTS

		event Action OnUIBuiltEvent;

		#endregion
		
		bool isUIBuilt { get; }
		
		UIElement[] GetAllCreatedUIElements();
		T GetUIElement<T>() where T : UIElement;
		T ShowUIElement<T>() where T : UIElement, IUIElementOnLayer;

		void BuildUI();
		void DestroyAll();

	}
}