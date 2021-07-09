using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture.UserInterface.Utils;

namespace VavilichevGD.Architecture.UserInterface {
    public abstract class UIPopup : UIElement, IUIPopup {


        [SerializeField] protected UILayerType _layer;
        [SerializeField] protected bool _isPreCached;
        [Space] [SerializeField] protected Button[] _buttonsClose;


        public UILayerType layer => _layer;
        public bool isPreCached => _isPreCached;
        public Button[] buttonsClose => _buttonsClose;

        private Canvas canvas { get; set; }



        #region AWAKE AND INITIALIZATION
        
        private void Awake() {
            if (isPreCached)
                InitPreCachedPopup();
        
            OnAwake();
        }
        
        private void InitPreCachedPopup() {
            InitCanvas();
            InitRaycaster();
        }
        
        private void InitCanvas() {
            canvas = gameObject.GetComponent<Canvas>();
            if (!canvas)
                canvas = gameObject.AddComponent<Canvas>();
        }
        
        private void InitRaycaster() {
            var raycaster = gameObject.GetComponent<GraphicRaycaster>();
            if (!raycaster)
                gameObject.AddComponent<GraphicRaycaster>();
        }

        protected virtual void OnAwake() { }
        
        #endregion

        

        #region SHOW

        public sealed override void Show() {
            if (isActive)
                return;

            OnPreShow();
            SubscribeOnCloseEvents();

            if (isPreCached) {
                transform.SetAsLastSibling();
                canvas.enabled = true;
            }

            isActive = true;
            gameObject.SetActive(true);
            OnPostShow();
            NotifyAboutShown();
        }

        private void SubscribeOnCloseEvents() {
            foreach (var button in buttonsClose)
                button.AddListener(OnCloseButtonClick);
        }

        #endregion



        #region HIDE

        public sealed override void HideInstantly() {
            if (!isActive)
                return;

            UnsubscribeFromCloseEvents();

            if (isPreCached) {
                canvas.enabled = false;
                gameObject.SetActive(false);
            }
            else
                Destroy(gameObject);

            isActive = false;
            OnPostHide();
        }
        
        private void UnsubscribeFromCloseEvents() {
            foreach (var button in buttonsClose)
                button.RemoveListener(OnCloseButtonClick);
        }

        #endregion

       

        #region EVENTS

        private void OnCloseButtonClick() {
            Hide();
        }

        #endregion

        public virtual void OnCreate() { }
        public virtual void OnInitialize() { }
        public virtual void OnStart() { }
    }
}