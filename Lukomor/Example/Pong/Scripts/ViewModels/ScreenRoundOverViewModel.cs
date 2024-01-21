using System;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class ScreenRoundOverViewModel : ScreenViewModel
    {
        public IReactiveProperty<string> WinnerText => _winnerText;
        public IReactiveProperty<string> CountText => _countText;

        private readonly SingleReactiveProperty<string> _winnerText = new();
        private readonly SingleReactiveProperty<string> _countText = new();
        private readonly GameSessionService _gameSessionsService;

        public ScreenRoundOverViewModel(GameSessionService gameSessionsService)
        {
            _gameSessionsService = gameSessionsService;

            gameSessionsService.RoundOver.Subscribe(_ =>
            {
                UpdateText();
            });
            
            UpdateText();
        }

        private void UpdateText()
        {
            var leftPlayerScore = _gameSessionsService.PlayerOneScore.Value;
            var rightPlayerScore = _gameSessionsService.PlayerTwoScore.Value;
            
            _countText.Value = $"{leftPlayerScore}:{rightPlayerScore}";
            _winnerText.Value = $"GOAL by  Player {_gameSessionsService.PlayerWhoScoredLastGoal.ToString()}";
        }

        public void RestartRound()
        {
            _gameSessionsService.RestartRound();
        }
    }
}