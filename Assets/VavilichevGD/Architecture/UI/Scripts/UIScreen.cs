using UnityEngine;

namespace VavilichevGD.Architecture.UserInterface {
	public abstract class UIScreen : UIElement, IUIScreen {

		[SerializeField] protected UILayerType _layer;
		[SerializeField] protected bool _showByDefault;

		public UILayerType layer => _layer;
		public bool showByDefault => _showByDefault;

		public virtual void OnCreate() { }
		public virtual void OnInitialize() { }
		public virtual void OnStart() { }
	}
}