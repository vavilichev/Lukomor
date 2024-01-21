using System;
using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class UIRootMainMenuViewModel : IViewModel
    {
         public IReactiveProperty<ScreenViewModel> OpenedScreen => _openedScreen;

        private readonly ReactiveProperty<ScreenViewModel> _openedScreen = new();
        private readonly Func<ScreenMainMenuViewModel> _screenMainMenuFactory;

        public UIRootMainMenuViewModel(Func<ScreenMainMenuViewModel> screenMainMenuFactory)
        {
            _screenMainMenuFactory = screenMainMenuFactory;
        }

        public void OpenMainMenu()
        {
            _openedScreen.Value = _screenMainMenuFactory();
        }
    }
}