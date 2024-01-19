using System;
using System.Reactive.Linq;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class PongScreenGameplayViewModel : PongScreenViewModel
    {
        public IReactiveProperty<string> GameScore => _gameScore;
        
        private readonly SingleReactiveProperty<string> _gameScore = new();

        public PongScreenGameplayViewModel(PongGameSessionService gameSessionsService)
        {
            gameSessionsService.LeftPlayerScore.Merge(gameSessionsService.RightPlayerScore).Subscribe(_ =>
            {
                _gameScore.Value = $"{gameSessionsService.LeftPlayerScore.Value}:{gameSessionsService.RightPlayerScore.Value}";
            });
        }
    }
}