using UnityEngine;

namespace Lukomor.Presentation.Views
{
    public abstract class View<TViewModel> : MonoBehaviour, IView<TViewModel> where TViewModel : ViewModel
    {
        #region Fields and properties

        public bool IsActive => gameObject.activeInHierarchy;
        public TViewModel ViewModel { get; private set; }
        
        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            ViewModel = GetComponent<TViewModel>();
            
            AwakeInternal();
        }

        protected virtual void AwakeInternal() { }

        #endregion

        #region Methods

        public virtual void Refresh() { }
        public virtual void Subscribe() { }
        public virtual void Unsubscribe() { }

        #endregion

    }
}