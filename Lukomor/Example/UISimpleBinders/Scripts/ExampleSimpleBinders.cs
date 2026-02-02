using Lukomor.MVVM;
using UnityEngine;

namespace Lukomor.Example.UISimpleBinders
{
    public class ExampleSimpleBinders : MonoBehaviour
    {
        [SerializeField] private View _uiRootView;
        
        private void Start()
        {
            var uiRootViewModel = new UIRootViewModel();
            _uiRootView.Bind(uiRootViewModel);
        }
    }
}