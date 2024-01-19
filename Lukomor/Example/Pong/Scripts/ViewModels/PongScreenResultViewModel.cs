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
        
        private readonly GameSessionsService _gameSessionsService;
        private readonly ScenesService _scenesService;
        private readonly Action _openGameplayScreen;

        public PongScreenResultViewModel(GameSessionsService gameSessionsService, ScenesService scenesService, Action openGameplayScreen)
        {
            _gameSessionsService = gameSessionsService;
            _scenesService = scenesService;
            _openGameplayScreen = openGameplayScreen;

            gameSessionsService.LeftPlayerScore.Merge(gameSessionsService.RightPlayerScore).Subscribe(_ =>
            {
                _countText.Value =
                    $"{gameSessionsService.LeftPlayerScore.Value}:{gameSessionsService.RightPlayerScore.Value}";
                _winnerText.Value = gameSessionsService.IsLastGoalByLeftPlayer.Value ? "Left player WIN!" : "Right player WIN!";
            });
        }
        
        public void HandleAgainButtonClick()
        {
            _openGameplayScreen();
            _gameSessionsService.RestartGame();
        }

        public void HandleMainMenuButtonClick()
        {
            _scenesService.LoadMainMenuScene();
        }
    }
}