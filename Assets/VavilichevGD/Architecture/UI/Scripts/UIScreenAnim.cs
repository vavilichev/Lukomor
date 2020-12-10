using UnityEngine;

namespace VavilichevGD.Architecture.UI {
	public abstract class UIScreenAnim : UIElementAnim, IUIElementOnLayer {

		[SerializeField] protected UILayerType m_layer;

		public UILayerType layer => this.m_layer;
	}
}