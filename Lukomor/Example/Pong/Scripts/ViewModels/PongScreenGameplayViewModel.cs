using System;
using Lukomor.Example.Pong.Scripts.Services;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class PongScreenGameplayViewModel : PongScreenViewModel
    {
        public IReactiveProperty<int> LeftPlayerScore;
        public IReactiveProperty<int> RightPlayerScore;
        
        private readonly GameSessionsService _gameSessionsService;
        private readonly Action<bool> _openResultScreen;
        private readonly Action _openPauseScreen;

        public PongScreenGameplayViewModel(GameSessionsService gameSessionsService, Action<bool> openResultScreen, Action openPauseScreen)
        {
            _gameSessionsService = gameSessionsService;
            _openPauseScreen = openPauseScreen;

            LeftPlayerScore = _gameSessionsService.LeftPlayerScore;
            RightPlayerScore = _gameSessionsService.RightPlayerScore;

            _gameSessionsService.Won.Subscribe(openResultScreen);
        }

        public void HandlePauseClick()
        {
            
        }
    }
}