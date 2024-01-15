using System.Runtime.CompilerServices;
using Lukomor.DI;

namespace Lukomor.Example.Pong
{
    public static class PongScreensRegistrations
    {
        public static void Register(DIContainer container)
        {
            container.Register(_ => ScreenGameplayViewModelFactory());
            container.Register(_ => ScreenPauseViewModelFactory());
            container.Register(_ => ScreenResultViewModelFactory());
            container.Register(_ => ScreenMainMenuViewModelFactory());
        }
        
        private static ScreenGameplayViewModel ScreenGameplayViewModelFactory()
        {
            return new ScreenGameplayViewModel();
        } 
        
        private static ScreenPauseViewModel ScreenPauseViewModelFactory()
        {
            return new ScreenPauseViewModel();
        } 
        
        private static ScreenResultViewModel ScreenResultViewModelFactory()
        {
            return new ScreenResultViewModel();
        } 
        
        private static ScreenMainMenuViewModel ScreenMainMenuViewModelFactory()
        {
            return new ScreenMainMenuViewModel();
        } 
    }
}