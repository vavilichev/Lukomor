using System;

namespace VavilichevGD.Architecture.Settings {
	public interface IAudioSettings : ISettings {

		#region EVENTS

		event Action OnVolumeSFXChangedEvent;
		event Action OnVolumeMusicChangedEvent;

		#endregion
		
		bool isEnabled { get; set; }
		bool isSFXEnabled { get; set; }
		bool isMusicEnabled { get; set; }
		float volumeSFX { get; set; }
		float volumeMusic { get; set; }
		
	}
}