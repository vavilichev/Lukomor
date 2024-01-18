using System;
using System.Reactive;
using System.Reactive.Linq;
using Lukomor.MVVM;

namespace Lukomor.Example.Pong
{
    public class BallViewModel : IViewModel
    {
        public IObservable<bool> IsActive { get; }
        public IObservable<Unit> PositionReset { get; }
        
        private readonly GameSessionsService _gameSessionsService;

        public BallViewModel(GameSessionsService gameSessionsService)
        {
            _gameSessionsService = gameSessionsService;

            IsActive = _gameSessionsService.IsPaused.Select(value => !value);
            PositionReset = gameSessionsService.RestartedRound;
        }
    }
}