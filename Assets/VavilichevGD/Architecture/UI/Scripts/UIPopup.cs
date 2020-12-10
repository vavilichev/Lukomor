using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture.UI.Utils;

namespace VavilichevGD.Architecture.UI {
	public abstract class UIPopup : UIElement, IUIPopup {


		[SerializeField] protected UILayerType m_layer;
		[SerializeField] protected bool m_isPreCached;
		[Space]
		[SerializeField] protected Button m_buttonClose;
		[SerializeField] protected Button m_buttonCloseAlt;


		public UILayerType layer => this.m_layer;
		public bool isPreCached => this.m_isPreCached;
		public Button buttonClose => this.m_buttonClose;
		public Button buttonCloseAlt => this.m_buttonCloseAlt;
		
		private Canvas canvas { get; set; }

        
        private void Awake() {
            if (this.isPreCached) 
                this.InitPreCachedPopup();

            this.OnAwake();
        }

        private void InitPreCachedPopup() {
            this.InitCanvas();
            this.InitRaycaster();
        }

        private void InitCanvas() {
            this.canvas = this.gameObject.GetComponent<Canvas>();
            if (!this.canvas) 
                this.canvas = this.gameObject.AddComponent<Canvas>();
        }

        private void InitRaycaster() {
            var raycaster = this.gameObject.GetComponent<GraphicRaycaster>();
            if (!raycaster) 
                this.gameObject.AddComponent<GraphicRaycaster>();
        }
        
        protected virtual void OnAwake() { }


        
        public sealed override void Show() {
            if (this.isActive)
                return;

            this.OnPreShow();
            this.SubscribeOnCloseEvents();
            
            if (this.isPreCached) {
                this.transform.SetAsLastSibling();
                this.canvas.enabled = true;
            }

            this.isActive = true;
            this.gameObject.SetActive(true);
            this.OnPostShow();
        }

        private void SubscribeOnCloseEvents() {
            if (this.buttonClose != null)
                this.buttonClose.AddListener(this.OnCloseButtonClick);
            if (this.buttonCloseAlt != null)
                this.buttonCloseAlt.AddListener(this.OnCloseButtonClick);
        }

        private void UnsubscribeFromCloseEvents() {
            if (this.buttonClose != null)
                this.buttonClose.RemoveListener(this.OnCloseButtonClick);
            if (this.buttonCloseAlt != null)
                this.buttonCloseAlt.RemoveListener(this.OnCloseButtonClick);
        }

        public sealed override void HideInstantly() {
            if (!this.isActive)
                return;

            this.UnsubscribeFromCloseEvents();

            if (this.isPreCached) {
                this.canvas.enabled = false;
                this.gameObject.SetActive(false);
            }
            else
                Destroy(this.gameObject);

            this.isActive = false;
            this.OnPostHide();
        }

        
        #region EVENTS

        private void OnCloseButtonClick() {
            this.Hide();
        }

        #endregion
	}
}