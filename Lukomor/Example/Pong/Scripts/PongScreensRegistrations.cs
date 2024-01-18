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
            container.RegisterSingleton(_ => new PongScreenResultViewModel());
            container.RegisterSingleton(c => new PongScreenGoalViewModel(c.Resolve<GameSessionsService>()));
        }
    }
}