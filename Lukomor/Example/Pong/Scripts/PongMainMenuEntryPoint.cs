using Lukomor.DI;
using Lukomor.MVVM;
using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class PongMainMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private View _uiRootView;
        
        public void Process(DIContainer container)
        {
            Debug.Log("Main Menu Entry Point");

            var scenesService = container.Resolve<PongScenesService>();

            container.RegisterSingleton(_ => new PongScreenMainMenuViewModel(scenesService.LoadGameplayScene));
            container.RegisterSingleton(container =>
                new PongUIMainMenuRootViewModel(() => container.Resolve<PongScreenMainMenuViewModel>()));

            var uiRootViewModel = container.Resolve<PongUIMainMenuRootViewModel>();
            _uiRootView.Bind(uiRootViewModel);
            
            uiRootViewModel.OpenMainMenu();
        }
    }
} 