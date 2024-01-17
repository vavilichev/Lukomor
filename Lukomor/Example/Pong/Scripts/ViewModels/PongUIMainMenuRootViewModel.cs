using System;
using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class PongUIMainMenuRootViewModel : IViewModel
    {
         public IReactiveProperty<PongScreenViewModel> OpenedScreen => _openedScreen;

        private readonly ReactiveProperty<PongScreenViewModel> _openedScreen = new();
        private readonly Func<PongScreenMainMenuViewModel> _screenMainMenuFactory;

        public PongUIMainMenuRootViewModel(Func<PongScreenMainMenuViewModel> screenMainMenuFactory)
        {
            _screenMainMenuFactory = screenMainMenuFactory;
        }

        public void OpenMainMenu()
        {
            _openedScreen.Value = _screenMainMenuFactory();
        }
    }
}