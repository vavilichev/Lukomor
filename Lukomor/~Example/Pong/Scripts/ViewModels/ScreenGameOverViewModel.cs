using System;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class ScreenGameOverViewModel : ScreenViewModel
    {
        public IReactiveProperty<string> WinnerText => _winnerText;
        public IReactiveProperty<string> CountText => _countText;
        
        private readonly SingleReactiveProperty<string> _winnerText = new();
        private readonly SingleReactiveProperty<string> _countText = new();
        
        private readonly GameSessionService _gameSessionsService;
        private readonly ScenesService _scenesService;

        public ScreenGameOverViewModel(GameSessionService gameSessionsService, ScenesService scenesService)
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
            var playerOneScore = _gameSessionsService.PlayerOneScore.Value;
            var playerTwoScore = _gameSessionsService.PlayerTwoScore.Value;
            var winner = playerOneScore > playerTwoScore ? PongPlayer.One : PongPlayer.Two;

            _countText.Value = $"{playerOneScore}:{playerTwoScore}";
            _winnerText.Value = $"Player {winner.ToString()} WIN!";
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