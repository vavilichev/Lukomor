using UnityEngine;

namespace VavilichevGD.Architecture.UI {
	public abstract class UIScreen : UIElement, IUIScreen {

		[SerializeField] protected UILayerType m_layer;
		[SerializeField] protected bool m_showByDefault;

		public UILayerType layer => this.m_layer;
		public bool showByDefault => this.m_showByDefault;

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