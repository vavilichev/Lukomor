using System;
using System.Reactive.Linq;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class ScreenGameplayViewModel : ScreenViewModel
    {
        public IReactiveProperty<string> GameScore => _gameScore;
        
        private readonly SingleReactiveProperty<string> _gameScore = new();

        public ScreenGameplayViewModel(GameSessionService gameSessionsService)
        {
            gameSessionsService.PlayerOneScore.Merge(gameSessionsService.PlayerTwoScore).Subscribe(_ =>
            {
                _gameScore.Value = $"{gameSessionsService.PlayerOneScore.Value}:{gameSessionsService.PlayerTwoScore.Value}";
            });
        }
    }
}