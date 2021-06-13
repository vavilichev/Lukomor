using UnityEngine;

namespace VavilichevGD.Architecture.UI {
	public abstract class UIScreenAnim : UIElementAnim, IUIElementOnLayer {

		[SerializeField] protected UILayerType m_layer;

		public UILayerType layer => this.m_layer;
		public void OnCreate() {
			throw new System.NotImplementedException();
		}

		public void OnInitialized() {
			throw new System.NotImplementedException();
		}

		public void OnStarted() {
			throw new System.NotImplementedException();
		}
	}
}