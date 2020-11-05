using UnityEngine;

namespace VavilichevGD.Core.Loadging {
    public abstract class LoadingScreenVisualBase : MonoBehaviour{

        #region DELEGATES

        public delegate void LoadingScreenVisualHandler(LoadingScreenVisualBase visualBase);
        
        public virtual event LoadingScreenVisualHandler OnShownEvent;
        public virtual event LoadingScreenVisualHandler OnHideStartEvent;
        public event LoadingScreenVisualHandler OnHiddenCompletelyEvent;

        #endregion
        
        public bool isActive { get; protected set; }


        public virtual void Show() {
            this.gameObject.SetActive(true);
            this.isActive = true;
            this.OnShownEvent?.Invoke(this);
        }

        public virtual void Hide() {
            this.OnHideStartEvent?.Invoke(this);
            this.HideInstantly();
        }
        
        public void HideInstantly() {
            this.gameObject.SetActive(false);
            this.isActive = false;
            this.OnHiddenCompletelyEvent?.Invoke(this);
        }

    }
}