using System;
using System.Reactive.Linq;
using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class UIRootGameplayViewModel : IViewModel
    {
        public IReactiveProperty<ScreenViewModel> OpenedScreen => _openedScreen;

        private readonly ReactiveProperty<ScreenViewModel> _openedScreen = new();
        private readonly Func<ScreenPauseViewModel> _screenPauseFactory;
        private readonly Func<ScreenGameOverViewModel> _screenGameOverFactory;
        private readonly Func<ScreenGameplayViewModel> _screenGameplayFactory;
        private readonly Func<ScreenRoundOverViewModel> _screenRoundOverFactory;
        private readonly GameSessionService _gameSessionsService;

        public UIRootGameplayViewModel(
            Func<ScreenPauseViewModel> screenPauseFactory,
            Func<ScreenGameOverViewModel> screenGameOverFactory,
            Func<ScreenGameplayViewModel> screenGameplayFactory,
            Func<ScreenRoundOverViewModel> screenRoundOverFactory,
            GameSessionService gameSessionsService)
        {
            _screenPauseFactory = screenPauseFactory;
            _screenGameOverFactory = screenGameOverFactory;
            _screenGameplayFactory = screenGameplayFactory;
            _screenRoundOverFactory = screenRoundOverFactory;
            _gameSessionsService = gameSessionsService;

            gameSessionsService.GameOver.Subscribe(_ => OpenGameOverScreen());
            gameSessionsService.RoundOver.Subscribe(_ => OpenRoundOverScreen());
            gameSessionsService.RoundRestarted.Merge(gameSessionsService.GameRestarted).Subscribe(_ => OpenGameplayScreen());
        }

        public void OpenGameplayScreen()
        {
            CloseOldScreen();

            _openedScreen.Value = _screenGameplayFactory();
        }

        public void HandlePauseButtonClick()
        {
            
            if (_gameSessionsService.IsPaused.Value)
            {
                _gameSessionsService.Unpause();
                OpenGameplayScreen();
            }
            else
            {
                _gameSessionsService.Pause();
                OpenPauseScreen();
            }
        }
        
        private void OpenPauseScreen()
        {
            CloseOldScreen();

            _openedScreen.Value = _screenPauseFactory();
        }

        private void OpenGameOverScreen()
        {
            CloseOldScreen();

            _openedScreen.Value = _screenGameOverFactory();
        }

        private void OpenRoundOverScreen()
        {
            CloseOldScreen();

            _openedScreen.Value = _screenRoundOverFactory();
        }

        private void CloseOldScreen()
        {
            _openedScreen.Value?.Close();
        }

    }
}