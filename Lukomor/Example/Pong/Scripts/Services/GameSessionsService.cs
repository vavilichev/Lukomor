using System;
using System.Reactive;
using System.Reactive.Linq;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class GameSessionsService
    {
        public IObservable<bool> Won { get; }
        public IReactiveProperty<int> LeftPlayerScore => _leftPlayerScore;
        public IReactiveProperty<int> RightPlayerScore => _rightPlayerScore;
        public IReactiveProperty<bool> IsLastGoalByLeftPlayer => _isLastGoalByLeftPlayer;
        public IReactiveProperty<bool> IsPaused => _isPaused;
        public IObservable<Unit> RestartedRound { get; }

        private readonly PongGameState _state;
        private readonly int _scoreLimit;
        private readonly Action<bool> _showGoalScreen;
        private readonly Action<bool> _showResultScreen;
        private readonly ReactiveProperty<int> _leftPlayerScore;
        private readonly ReactiveProperty<int> _rightPlayerScore;
        private readonly ReactiveProperty<bool> _isPaused;
        private readonly ReactiveProperty<bool> _isLastGoalByLeftPlayer = new();
        private event Action<bool> _won;
        private event Action<Unit> _restartedRound;

        public GameSessionsService(PongGameState gameState, int scoreLimit, Action<bool> showGoalScreen, Action<bool> showResultScreen)
        {
            _state = gameState;
            _scoreLimit = scoreLimit;
            _showGoalScreen = showGoalScreen;
            _showResultScreen = showResultScreen;

            _leftPlayerScore = new ReactiveProperty<int>(_state.LeftPlayerScore);
            _rightPlayerScore = new ReactiveProperty<int>(_state.RightPlayerScore);
            _isPaused = new ReactiveProperty<bool>(false);

            Won = Observable.FromEvent<bool>(a => _won += a, a => _won -= a);
            RestartedRound = Observable.FromEvent<Unit>(a => _restartedRound += a, a => _restartedRound -= a);
        }

        public void RegisterGoal(bool leftPlayer)
        {
            _isLastGoalByLeftPlayer.Value = leftPlayer;

            Pause();
            
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
                _showResultScreen(leftPlayer);
                _won?.Invoke(true);
            }
            else if (_state.RightPlayerScore >= _scoreLimit)
            {
                _showResultScreen(leftPlayer);
                _won?.Invoke(false);
            }
            else
            {
                _showGoalScreen(leftPlayer);
            }
        }

        public void RestartRound()
        {
            Unpause();
            
            _restartedRound?.Invoke(Unit.Default);
        }

        public void RestartGame()
        {
            
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