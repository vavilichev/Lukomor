using System;
using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class PongUIRootViewModel : IViewModel
    {
        public IReactiveProperty<PongScreenViewModel> OpenedScreen => _openedScreen;

        private readonly ReactiveProperty<PongScreenViewModel> _openedScreen = new();
        private readonly Func<PongScreenPauseViewModel> _screenPauseFactory;
        private readonly Func<PongScreenResultViewModel> _screenResultFactory;
        private readonly Func<PongScreenGameplayViewModel> _screenGameplayFactory;
        private readonly Func<PongScreenGoalViewModel> _screenGoalFactory;
        private readonly PongGameSessionService _gameSessionsService;

        public PongUIRootViewModel(
            Func<PongScreenPauseViewModel> screenPauseFactory,
            Func<PongScreenResultViewModel> screenResultFactory,
            Func<PongScreenGameplayViewModel> screenGameplayFactory,
            Func<PongScreenGoalViewModel> screenGoalFactory,
            PongGameSessionService gameSessionsService)
        {
            _screenPauseFactory = screenPauseFactory;
            _screenResultFactory = screenResultFactory;
            _screenGameplayFactory = screenGameplayFactory;
            _screenGoalFactory = screenGoalFactory;
            _gameSessionsService = gameSessionsService;

            gameSessionsService.GameOver.Subscribe(_ => OpenResultScreen());
            gameSessionsService.RoundOver.Subscribe(_ => OpenGoalScreen());
            gameSessionsService.RoundRestarted.Subscribe(_ => OpenGameplayScreen());
            gameSessionsService.GameRestarted.Subscribe(_ => OpenGameplayScreen());
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

        public void OpenGoalScreen()
        {
            CloseOldScreen();

            _openedScreen.Value = _screenGoalFactory();
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

        private void CloseOldScreen()
        {
            _openedScreen.Value?.Close();
        }

    }
}