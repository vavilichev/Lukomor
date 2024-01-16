using System;
using System.Reactive.Linq;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong.Scripts.Services
{
    public class GameSessionsService
    {
        public IObservable<bool> Won { get; }
        public IReactiveProperty<int> LeftPlayerScore => _leftPlayerScore;
        public IReactiveProperty<int> RightPlayerScore => _rightPlayerScore;
        public IReactiveProperty<bool> IsPaused => _isPaused;

        private readonly PongGameState _state;
        private readonly int _scoreLimit;
        private readonly ReactiveProperty<int> _leftPlayerScore;
        private readonly ReactiveProperty<int> _rightPlayerScore;
        private readonly ReactiveProperty<bool> _isPaused;
        private event Action<bool> _won;

        public GameSessionsService(PongGameState gameState, int scoreLimit)
        {
            _state = gameState;
            _scoreLimit = scoreLimit;

            _leftPlayerScore = new ReactiveProperty<int>(_state.LeftPlayerScore);
            _rightPlayerScore = new ReactiveProperty<int>(_state.RightPlayerScore);
            _isPaused = new ReactiveProperty<bool>(false);

            Won = Observable.FromEvent<bool>(a => _won += a, a => _won -= a);
        }

        public void RegisterGoal(bool leftPlayer)
        {
            if (leftPlayer)
            {
                _state.LeftPlayerScore++;
                _leftPlayerScore.Value = _state.LeftPlayerScore;
            }
            else
            {
                _state.RightPlayerScore++;
                _rightPlayerScore.Value = _state.RightPlayerScore;
            }

            if (_state.LeftPlayerScore >= _scoreLimit)
            {
                _won?.Invoke(true);
            }
            else if (_state.RightPlayerScore >= _scoreLimit)
            {
                _won?.Invoke(false);
            }
        }

        public void Pause()
        {
            _isPaused.Value = true;
        }

        public void Unpause()
        {
            _isPaused.Value = false;
        }
    }
}