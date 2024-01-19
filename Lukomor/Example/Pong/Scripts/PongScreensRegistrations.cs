using Lukomor.DI;

namespace Lukomor.Example.Pong
{
    public class PongScreensRegistrations
    {
        public void Register(DIContainer container)
        {
            container.RegisterSingleton(c => new PongScreenGameplayViewModel(
                c.Resolve<PongGameSessionService>())
            );

            container.RegisterSingleton(c => new PongScreenPauseViewModel(c.Resolve<PongGameSessionService>()));

            container.RegisterSingleton(c => new PongScreenResultViewModel(
                c.Resolve<PongGameSessionService>(),
                c.Resolve<PongScenesService>()));
            
            container.RegisterSingleton(c => new PongScreenGoalViewModel(
                c.Resolve<PongGameSessionService>()));
        }
    }
}