using System;
using System.Collections.Generic;

namespace VavilichevGD.Architecture.UI {
	public interface IUIContainer {

		#region EVENTS

		event Action OnUIBuiltEvent;

		#endregion

		IUIContainerConfig config { get; }
		bool isBuilt { get; set; }
		
		Dictionary<Type, IUIElement> createdElementsMap { get; }
		Dictionary<Type, string> prefabPathsMap { get; }
		Dictionary<Type, IUIPopup> cachedPopupsMap { get; }
		List<IArchitectureCaptureEvents> elementsForArchitectureNotifications { get; }

		void BuildUI();
		void Destroy();

		void SendEventOnCreate();
		void SendEventOnInitialized();
		void SendEventOnStarted();
		
		T GetUIElement<T>() where T : UIElement;
		T ShowUIElement<T>() where T : UIElement, IUIElementOnLayer;
	}
}