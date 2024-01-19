using System;
using System.Reactive;
using System.Reactive.Linq;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class PongGameSessionService
    {
        public IObservable<Unit> GameOver { get; }
        public IObservable<Unit> RoundOver { get; }
        public IObservable<Unit> RoundRestarted { get; }
        public IObservable<Unit> GameRestarted { get; }
        public IReactiveProperty<int> LeftPlayerScore => _leftPlayerScore;
        public IReactiveProperty<int> RightPlayerScore => _rightPlayerScore;
        public IReactiveProperty<bool> IsPaused => _isPaused;
        public bool LastGoalByLeftPlayer { get; private set; }
        
        private readonly int _scoreLimit;
        private readonly ReactiveProperty<int> _leftPlayerScore;
        private readonly ReactiveProperty<int> _rightPlayerScore;
        private readonly ReactiveProperty<bool> _isPaused;
        private event Action<Unit> _gameOver;
        private event Action<Unit> _roundOver;
        private event Action<Unit> _restartedRound;
        private event Action<Unit> _restartedGame;
        
        public PongGameSessionService(int scoreLimit)
        {
            _scoreLimit = scoreLimit;
            _leftPlayerScore = new ReactiveProperty<int>(0);
            _rightPlayerScore = new ReactiveProperty<int>(0);
            _isPaused = new ReactiveProperty<bool>(false);

            GameOver = Observable.FromEvent<Unit>(a => _gameOver += a, a => _gameOver -= a);
            RoundOver = Observable.FromEvent<Unit>(a => _roundOver += a, a => _roundOver -= a);
            RoundRestarted = Observable.FromEvent<Unit>(a => _restartedRound += a, a => _restartedRound -= a);
            GameRestarted = Observable.FromEvent<Unit>(a => _restartedGame += a, a => _restartedGame -= a);
        }

        public void RegisterGoal(bool leftPlayer)
        {
            LastGoalByLeftPlayer = leftPlayer;
            
            Pause();
            
            if (leftPlayer)
            {
                _leftPlayerScore.Value++;
            }
            else
            {
                _rightPlayerScore.Value++;
            }
            
            if (_leftPlayerScore.Value >= _scoreLimit || _rightPlayerScore.Value >= _scoreLimit)
            {
                _gameOver?.Invoke(Unit.Default);
            }
            else
            {
                _roundOver?.Invoke(Unit.Default);
            }
        }

        public void RestartRound()
        {
            Unpause();
            
            _restartedRound?.Invoke(Unit.Default);
        }

        public void RestartGame()
        {
            _leftPlayerScore.Value = 0;
            _rightPlayerScore.Value = 0;
            
            Unpause();
            
            _restartedGame?.Invoke(Unit.Default);
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