using UnityEngine;

namespace VavilichevGD.Architecture.UserInterface {
	public sealed class UILayer : UIElement {

		[SerializeField] private UILayerType _layer;

		public UILayerType layer => _layer;

	}
}