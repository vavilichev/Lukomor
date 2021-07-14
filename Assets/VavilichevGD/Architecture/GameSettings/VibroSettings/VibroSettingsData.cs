using System;

namespace VavilichevGD.Architecture.Settings {
	[Serializable]
	public class VibroSettingsData {
		public bool isVibroEnabled;

		public VibroSettingsData() {
			isVibroEnabled = true;
		}
	}
}