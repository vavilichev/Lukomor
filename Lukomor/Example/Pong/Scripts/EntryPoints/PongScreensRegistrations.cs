using Lukomor.DI;

namespace Lukomor.Example.Pong
{
    public class PongScreensRegistrations
    {
        public void Register(DIContainer container)
        {
            container.RegisterSingleton(c => new ScreenGameplayViewModel(
                c.Resolve<GameSessionService>())
            );

            container.RegisterSingleton(c => new ScreenPauseViewModel(c.Resolve<GameSessionService>()));

            container.RegisterSingleton(c => new ScreenGameOverViewModel(
                c.Resolve<GameSessionService>(),
                c.Resolve<PongScenesService>()));
            
            container.RegisterSingleton(c => new ScreenRoundOverViewModel(
                c.Resolve<GameSessionService>()));
        }
    }
}