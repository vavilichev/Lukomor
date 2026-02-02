using Lukomor.MVVM;
using UnityEngine;

namespace Lukomor.Example.World
{
    public class ExampleWorld : MonoBehaviour
    {
        [SerializeField] private View _uiRootView;
        [SerializeField] private View _worldRootView;
        
        private void Start()
        {
            var objectsService = new ObjectsService();
            var uiRootViewModel = new UIRootViewModel(objectsService);
            var worldRootViewModel = new WorldRootViewModel(objectsService);
            
            _uiRootView.Bind(uiRootViewModel);
            _worldRootView.Bind(worldRootViewModel);
        }
    }
}