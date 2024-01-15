using System;
using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class UIRootViewModel : IViewModel
    {
        public IReactiveProperty<ScreenViewModel> OpenedScreen => _openedScreen;

        private readonly ReactiveProperty<ScreenViewModel> _openedScreen = new();
        private readonly Func<ScreenMainMenuViewModel> _screenMainMenuFactory;
        private readonly Func<ScreenPauseViewModel> _screenPauseFactory;
        private readonly Func<ScreenResultViewModel> _screenResultFactory;
        private readonly Func<ScreenGameplayViewModel> _screenGameplayFactory;

        public UIRootViewModel(
            Func<ScreenMainMenuViewModel> screenMainMenuFactory,
            Func<ScreenPauseViewModel> screenPauseFactory,
            Func<ScreenResultViewModel> screenResultFactory,
            Func<ScreenGameplayViewModel> screenGameplayFactory)
        {
            _screenMainMenuFactory = screenMainMenuFactory;
            _screenPauseFactory = screenPauseFactory;
            _screenResultFactory = screenResultFactory;
            _screenGameplayFactory = screenGameplayFactory;
        }

        public void OpenMainMenuScreen()
        {
            CloseOldScreen();

            _openedScreen.Value = _screenMainMenuFactory();
        }

        public void OpenPauseScreen()
        {
            CloseOldScreen();

            _openedScreen.Value = _screenPauseFactory();
        }

        public void OpenResultScreen()
        {
            CloseOldScreen();

            _openedScreen.Value = _screenResultFactory();
        }

        public void OpenGameplayScreen()
        {
            CloseOldScreen();

            _openedScreen.Value = _screenMainMenuFactory();
        }

        private void CloseOldScreen()
        {
            _openedScreen.Value?.Close();
            _openedScreen.Value = null;
        }

    }
}