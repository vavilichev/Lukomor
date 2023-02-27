namespace Lukomor.UI
{
    public abstract class ViewModel : DiViewModel
    {
        public IView View { get; private set; }
        public bool IsActive => View.IsActive;
        
        protected UserInterface UI { get; private set; }
        
        private void Awake()
        {
            UI = GetComponentInParent<UserInterface>();
            View = GetComponent<IView>();
            
            AwakeInternal();
        }

        protected virtual void AwakeInternal() { }
    }
}