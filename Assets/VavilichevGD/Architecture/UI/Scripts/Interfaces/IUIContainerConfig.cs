namespace VavilichevGD.Architecture.UI {
	public interface IUIContainerConfig {
		IUIElementOnLayer[] prefabs { get; }

		IUIElementOnLayer[] BuildElementsOnLayer();
	}
}