using UnityEngine;

namespace Lukomor.UI
{
    public abstract class View<TViewModel> : MonoBehaviour, IView<TViewModel> where TViewModel : ViewModel
    {
        public bool IsActive => gameObject.activeInHierarchy;
        public TViewModel ViewModel { get; private set; }
        
        protected UserInterface UI { get; private set; }
        

        private void Awake()
        {
            ViewModel = GetComponent<TViewModel>();
            UI = GetComponentInParent<UserInterface>();
            
            AwakeInternal();
        }

        protected virtual void AwakeInternal() { }
    }
}