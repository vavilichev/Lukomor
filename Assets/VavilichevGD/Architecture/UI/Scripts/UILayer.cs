using UnityEngine;

namespace VavilichevGD.Architecture.UI {
	public sealed class UILayer : UIElement {
		
		[SerializeField] private UILayerType m_layer;

		public UILayerType layer => this.m_layer;

	}
}