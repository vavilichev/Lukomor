using System;
using System.Reactive.Linq;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class PongScreenResultViewModel : PongScreenViewModel
    {
        public IReactiveProperty<string> WinnerText => _winnerText;
        public IReactiveProperty<string> CountText => _countText;
        
        private readonly SingleReactiveProperty<string> _winnerText = new();
        private readonly SingleReactiveProperty<string> _countText = new();
        
        private readonly PongGameSessionService _gameSessionsService;
        private readonly PongScenesService _scenesService;

        public PongScreenResultViewModel(PongGameSessionService gameSessionsService, PongScenesService scenesService)
        {
            _gameSessionsService = gameSessionsService;
            _scenesService = scenesService;
            
            gameSessionsService.GameOver.Subscribe(_ =>
            {
                UpdateText();
            });
            
            UpdateText();
        }

        private void UpdateText()
        {
            var leftPlayerScore = _gameSessionsService.LeftPlayerScore.Value;
            var rightPlayerScore = _gameSessionsService.RightPlayerScore.Value;
            var isLeftPlayerWinner = leftPlayerScore > rightPlayerScore;

            _countText.Value = $"{leftPlayerScore}:{rightPlayerScore}";

            _winnerText.Value = isLeftPlayerWinner
                ? "Left player WIN!"
                : "Right player WIN!";
        }
        
        public void HandleAgainButtonClick()
        {
            _gameSessionsService.RestartGame();
        }

        public void HandleMainMenuButtonClick()
        {
            _scenesService.LoadMainMenuScene();
        }
    }
}