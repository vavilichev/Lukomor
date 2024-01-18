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

        public PongUIRootViewModel(
            Func<PongScreenPauseViewModel> screenPauseFactory,
            Func<PongScreenResultViewModel> screenResultFactory,
            Func<PongScreenGameplayViewModel> screenGameplayFactory,
            Func<PongScreenGoalViewModel> screenGoalFactory)
        {
            _screenPauseFactory = screenPauseFactory;
            _screenResultFactory = screenResultFactory;
            _screenGameplayFactory = screenGameplayFactory;
            _screenGoalFactory = screenGoalFactory;

            // gameSessionsService.IsPaused.Skip(1).Subscribe(isPaused =>
            // {
            //     if (isPaused)
            //     {
            //         OpenPauseScreen();
            //     }
            //     else
            //     {
            //         OpenGameplayScreen();
            //     }
            // });
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

        public void OpenGoalScreen(bool isLeftPlayerWinner)
        {
            CloseOldScreen();

            _openedScreen.Value = _screenGoalFactory();
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