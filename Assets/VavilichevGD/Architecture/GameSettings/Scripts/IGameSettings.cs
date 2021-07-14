namespace VavilichevGD.Architecture.Settings {
	public interface IGameSettings : ISettings {
		IAudioSettings audioSettings { get; }
		IVibroSettings vibroSettings { get; }
	}
}