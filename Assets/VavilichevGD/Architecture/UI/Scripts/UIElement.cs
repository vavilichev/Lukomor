using System;
using UnityEngine;

namespace VavilichevGD.Architecture.UserInterface {
	public abstract class UIElement : MonoBehaviour, IUIElement {

		#region CONSTANTS

		/// <summary>
		/// Use this trigger name for Hide animation.
		/// </summary>
		private static readonly int triggerHide = Animator.StringToHash("hide");

		#endregion

		#region EVENTS

		public event Action<IUIElement> OnElementHideStartedEvent;
		public event Action<IUIElement> OnElementHiddenCompletelyEvent;
		public event Action<IUIElement> OnElementShownEvent;
		public event Action<IUIElement> OnElementDestroyedEvent;

		#endregion

		[SerializeField] protected Animator _animator;

		public bool isActive { get; protected set; } = true;
		public UIController uiController => UI.controller;



		#region SHOW

		public virtual void Show() {
			if (isActive)
				return;

			OnPreShow();
			gameObject.SetActive(true);
			isActive = true;
			OnPostShow();
			NotifyAboutShown();
		}

		protected virtual void OnPreShow() { }
		protected virtual void OnPostShow() { }

		protected void NotifyAboutShown() {
			OnElementShownEvent?.Invoke(this);
		}

		#endregion


		
		#region HIDE

		public virtual void Hide() {
			if (!isActive)
				return;

			NotifyAboutHideStarted();

			if (_animator != null) {
				_animator.SetTrigger(triggerHide);
				return;
			}

			HideInstantly();
		}

		protected void NotifyAboutHideStarted() {
			OnPreHide();
			OnElementHideStartedEvent?.Invoke(this);
		}

		public virtual void HideInstantly() {
			if (!isActive)
				return;

			isActive = false;
			gameObject.SetActive(false);
			OnPostHide();
			OnElementHiddenCompletelyEvent?.Invoke(this);
		}

		protected virtual void OnPreHide() { }
		protected virtual void OnPostHide() { }

		#endregion



		#region CALLBACKS

		/// <summary>
		/// Use this handle for triggering end of Hide animation.
		/// </summary>
		protected virtual void Handle_AnimationOutOver() {
			HideInstantly();
		}

		#endregion


#if UNITY_EDITOR
		protected virtual void Reset() {
			if (_animator == null)
				_animator = GetComponent<Animator>();
		}
#endif

	}
}