using System;
using System.Reactive.Linq;
using Lukomor.Example.Pong.Scripts.Services;
using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class PongUIRootViewModel : IViewModel
    {
        public IReactiveProperty<PongScreenViewModel> OpenedScreen => _openedScreen;

        private readonly ReactiveProperty<PongScreenViewModel> _openedScreen = new();
        private readonly Func<PongScreenMainMenuViewModel> _screenMainMenuFactory;
        private readonly Func<PongScreenPauseViewModel> _screenPauseFactory;
        private readonly Func<PongScreenResultViewModel> _screenResultFactory;
        private readonly Func<PongScreenGameplayViewModel> _screenGameplayFactory;

        public PongUIRootViewModel(
            Func<PongScreenMainMenuViewModel> screenMainMenuFactory,
            Func<PongScreenPauseViewModel> screenPauseFactory,
            Func<PongScreenResultViewModel> screenResultFactory,
            Func<PongScreenGameplayViewModel> screenGameplayFactory,
            GameSessionsService gameSessionsService)
        {
            _screenMainMenuFactory = screenMainMenuFactory;
            _screenPauseFactory = screenPauseFactory;
            _screenResultFactory = screenResultFactory;
            _screenGameplayFactory = screenGameplayFactory;

            gameSessionsService.IsPaused.Skip(1).Subscribe(isPaused =>
            {
                if (isPaused)
                {
                    OpenPauseScreen();
                }
                else
                {
                    OpenGameplayScreen();
                }
            });
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

        public void OpenResultScreen(bool isLeftPlayerWinner)
        {
            CloseOldScreen();

            _openedScreen.Value = _screenResultFactory();
        }

        public void OpenGameplayScreen()
        {
            CloseOldScreen();

            _openedScreen.Value = _screenGameplayFactory();
        }

        private void CloseOldScreen()
        {
            _openedScreen.Value?.Close();
        }

    }
}