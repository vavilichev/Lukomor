using System;

namespace VavilichevGD.Architecture.Settings {
	public interface IVibroSettings : ISettings {

		#region EVENTS

		event Action OnVibroStateChangedEvent;

		#endregion
		
		bool isEnabled { get; }
	}
}