using System;
using UnityEngine;

namespace VavilichevGD.Architecture {
	public interface IArchitectureComponent : IArchitectureCaptureEvents {
		
		#region EVENTS

		event Action OnInitializedEvent; 

		#endregion
        
		State state { get; }
		bool isInitialized { get; }
		bool isLoggingEnabled { get; set; }

		Coroutine InitializeWithRoutine();
	}
}