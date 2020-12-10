using UnityEngine;

namespace VavilichevGD.Architecture.UI {
	public abstract class UIElementAnim : UIElement {

		#region CONSTANTS

		private static readonly int triggerHide = Animator.StringToHash("hide");

		#endregion

		
		[SerializeField] protected Animator animator;


		protected virtual void Handle_AnimationOutOver() {
			this.HideInstantly();
		}

		public override void Hide() {
			if (!this.isActive) {
				return;
			}

			this.animator.SetTrigger(triggerHide);
			this.NotifyAboutHideStarted();
		}


#if UNITY_EDITOR
		protected virtual void Reset() {
			if (this.animator == null) 
				this.animator = this.GetComponent<Animator>();
		}
#endif
		
	}
}