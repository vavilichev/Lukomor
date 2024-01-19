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
        private readonly PongGameSessionService _gameSessionsService;

        public PongScreenGoalViewModel(PongGameSessionService gameSessionsService)
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
            var leftPlayerScore = _gameSessionsService.LeftPlayerScore.Value;
            var rightPlayerScore = _gameSessionsService.RightPlayerScore.Value;
            var isLeftPlayerWinner = _gameSessionsService.LastGoalByLeftPlayer;
            
            _countText.Value = $"{leftPlayerScore}:{rightPlayerScore}";
            _winText.Value = isLeftPlayerWinner ? "Left player GOAL!" : "Right player GOAL!";
        }

        public void Continue()
        {
            _gameSessionsService.RestartRound();
        }
    }
}