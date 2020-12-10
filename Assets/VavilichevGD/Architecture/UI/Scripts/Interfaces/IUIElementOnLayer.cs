namespace VavilichevGD.Architecture.UI {
	public partial interface IUIElementOnLayer : IUIElement {
		UILayerType layer { get; }
	}
}