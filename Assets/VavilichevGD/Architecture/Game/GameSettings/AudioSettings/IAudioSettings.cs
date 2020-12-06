using System;

namespace VavilichevGD.Architecture.Settings {
	public interface IAudioSettings : ISettings {

		#region EVENTS

		event Action OnVolumeSFXChangedEvent;
		event Action OnVolumeMusicChangedEvent;

		#endregion
		
		bool isEnabled { get; }
		bool isSFXEnabled { get; }
		bool isMusicEnabled { get; }
		float volumeSFX { get; }
		float volumeMusic { get; }
		
	}
}