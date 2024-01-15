using System;
using System.Runtime.CompilerServices;
using Lukomor.DI;
using UnityEditor.Build.Content;

namespace Lukomor.Example.Pong
{
    public static class PongScreensRegistrations
    {
        public static void Register(DIContainer container)
        {
            container.Register(_ => ScreenGameplayViewModelFactory());
            container.Register(_ => ScreenPauseViewModelFactory());
            container.Register(_ => ScreenResultViewModelFactory());
            container.Register(_ =>
                ScreenMainMenuViewModelFactory(() => container.Resolve<PongScreenPauseViewModel>()));
        }
        
        private static PongScreenGameplayViewModel ScreenGameplayViewModelFactory()
        {
            return new PongScreenGameplayViewModel();
        } 
        
        private static PongScreenPauseViewModel ScreenPauseViewModelFactory()
        {
            return new PongScreenPauseViewModel();
        } 
        
        private static PongScreenResultViewModel ScreenResultViewModelFactory()
        {
            return new PongScreenResultViewModel();
        } 
        
        private static PongScreenMainMenuViewModel ScreenMainMenuViewModelFactory(Action showPauseScreen)
        {
            return new PongScreenMainMenuViewModel(showPauseScreen);
        } 
    }
}