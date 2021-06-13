namespace VavilichevGD.Architecture.UI {
	public partial interface IUIElementOnLayer : IUIElement, IArchitectureCaptureEvents {
		UILayerType layer { get; }
	}
}