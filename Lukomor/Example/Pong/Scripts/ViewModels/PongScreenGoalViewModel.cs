using System;
using System.Reactive.Linq;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class PongScreenGoalViewModel : PongScreenViewModel
    {
        public IReactiveProperty<string> WinText => _winText;
        public IReactiveProperty<string> CountText => _countText;

        private readonly SingleReactiveProperty<string> _winText = new();
        private readonly SingleReactiveProperty<string> _countText = new();
        private readonly GameSessionsService _gameSessionsService;
        private readonly Action _openGameplayScreen;

        public PongScreenGoalViewModel(GameSessionsService gameSessionsService, Action openGameplayScreen)
        {
            _gameSessionsService = gameSessionsService;
            _openGameplayScreen = openGameplayScreen;

            gameSessionsService.LeftPlayerScore.Merge(gameSessionsService.RightPlayerScore).Subscribe(_ =>
            {
                _countText.Value =
                    $"{gameSessionsService.LeftPlayerScore.Value}:{gameSessionsService.RightPlayerScore.Value}";
                _winText.Value = gameSessionsService.IsLastGoalByLeftPlayer.Value ? "Left player GOAL!" : "Right player GOAL!";
            });
        }

        public void Continue()
        {
            _openGameplayScreen();
            _gameSessionsService.RestartRound();
        }
    }
}