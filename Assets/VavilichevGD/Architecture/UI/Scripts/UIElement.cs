using System;
using UnityEngine;

namespace VavilichevGD.Architecture.UI {
	public abstract class UIElement : MonoBehaviour, IUIElement {

		#region EVENTS

		public event Action<IUIElement> OnElementHideStartedEvent;
		public event Action<IUIElement> OnElementHiddenCompletelyEvent;
		public event Action<IUIElement> OnElementShownEvent;
		public event Action<IUIElement> OnElementDestroyedEvent;

		#endregion

		private static IUIController m_uiController;

		public bool isActive { get; protected set; } = true;
		
		protected static IUIController uiController {
			get {
				if (m_uiController == null)
					m_uiController = Game.uiController;
				return m_uiController;
			}
		}

		public virtual void Show() {
			if (this.isActive)
				return;
			
			this.OnPreShow();
			this.gameObject.SetActive(true);
			this.isActive = true;
			this.OnPostShow();
			this.OnElementShownEvent?.Invoke(this);
		}

		protected virtual void OnPreShow() { }
		protected virtual void OnPostShow() { }
		

		public virtual void Hide() {
			if (this.isActive) {
				this.NotifyAboutHideStarted();
				this.HideInstantly();
			}
		}

		protected void NotifyAboutHideStarted() {
			this.OnElementHideStartedEvent?.Invoke(this);
		}

		public virtual void HideInstantly() {
			if (!this.isActive)
				return;
			
			this.OnPreHide();
			this.isActive = false;
			this.gameObject.SetActive(false);
			this.OnPostHide();
			this.OnElementHiddenCompletelyEvent?.Invoke(this);
		}
		
		protected virtual void OnPreHide() { }
		protected virtual void OnPostHide() { }
		
	}
}