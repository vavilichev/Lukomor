using UnityEngine;

namespace VavilichevGD.Architecture.UI {
	public abstract class UIScreenAnim : UIElementAnim, IUIElementOnLayer {

		[SerializeField] protected UILayerType m_layer;

		public UILayerType layer => this.m_layer;
		public void OnCreate() {
			throw new System.NotImplementedException();
		}

		public void OnInitialize() {
			throw new System.NotImplementedException();
		}

		public void OnStart() {
			throw new System.NotImplementedException();
		}
	}
}