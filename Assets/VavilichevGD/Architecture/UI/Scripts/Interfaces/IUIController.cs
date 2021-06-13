using System;

namespace VavilichevGD.Architecture.UI {
	public interface IUIController {

		#region EVENTS

		event Action OnBuiltEvent;

		#endregion
		
		IUIContainer container { get; set; }
	}
}