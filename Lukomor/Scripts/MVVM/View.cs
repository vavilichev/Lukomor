using UnityEngine;

namespace Lukomor.MVVM
{
    public class View : MonoBehaviour
    {
        [SerializeField] private string _viewModelTypeFullName;
        
        public void Bind(IViewModel viewModel)
        {
            
        }

        public void Destroy()
        {
            // TODO: make it more flexible
            Destroy(gameObject);
        }
    }
}