using Lukomor.DI;

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
            
            container.RegisterSingleton(c => new PongScreenResultViewModel(
                c.Resolve<GameSessionsService>(),
                c.Resolve<ScenesService>(),
                c.Resolve<PongUIRootViewModel>().OpenGameplayScreen));
            
            container.RegisterSingleton(c => new PongScreenGoalViewModel(
                c.Resolve<GameSessionsService>(),
                c.Resolve<PongUIRootViewModel>().OpenGameplayScreen));
        }
    }
}