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
            var scenesService = container.Resolve<PongScenesService>();

            container.RegisterSingleton(_ => new ScreenMainMenuViewModel(scenesService.LoadGameplayScene));
            container.RegisterSingleton(c =>
                new UIRootMainMenuViewModel(() => c.Resolve<ScreenMainMenuViewModel>()));

            var uiRootViewModel = container.Resolve<UIRootMainMenuViewModel>();
            _uiRootView.Bind(uiRootViewModel);
            
            uiRootViewModel.OpenMainMenu();
        }
    }
} 