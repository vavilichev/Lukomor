using Lukomor.DI;
using UnityEngine;

namespace Lukomor.UI
{
    public abstract class DiViewModel : MonoBehaviour
    {
        protected DiContainer DiContainer { get; private set; }

        private bool wasConstructed => DiContainer != null;

        public void Construct(DiContainer diContainer)
        {
            if (!wasConstructed)
            {
                DiContainer = diContainer;
                
                ConstructChildren();

                OnConstructed();
            }
        }

        private void ConstructChildren()
        {
            var diMonoBehaviours = gameObject.GetComponentsInChildren<DiViewModel>(true);

            foreach (DiViewModel diMono in diMonoBehaviours)
            {
                diMono.Construct(DiContainer);
            }
        }

        protected virtual void OnConstructed() { }
    }
}