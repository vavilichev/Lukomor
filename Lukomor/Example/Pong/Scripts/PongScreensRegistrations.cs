using Lukomor.DI;
using Lukomor.Example.Pong.Scripts.Services;

namespace Lukomor.Example.Pong
{
    public class PongScreensRegistrations
    {
        public void Register(DIContainer container)
        {
            container.RegisterSingleton(c => new PongScreenGameplayViewModel(
                c.Resolve<GameSessionsService>(),
                c.Resolve<PongUIRootViewModel>().OpenResultScreen, 
                c.Resolve<PongUIRootViewModel>().OpenPauseScreen)
            );

            container.RegisterSingleton(_ => new PongScreenPauseViewModel());
            container.RegisterSingleton(_ => new PongScreenResultViewModel());
            
            container.RegisterSingleton(c => new PongScreenMainMenuViewModel
                (c.Resolve<PongUIRootViewModel>().OpenPauseScreen)
            );
        }
    }
}