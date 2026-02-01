using Lukomor.MVVM;
using UnityEngine;

namespace Lukomor.Example.Collections
{
    public class ExampleCollectionBinders : MonoBehaviour
    {
        [SerializeField] private View _uiRootView;
        
        private void Start()
        {
            var uiRootViewModel = new UIRootViewModel();
            _uiRootView.Bind(uiRootViewModel);
        }
    }
}