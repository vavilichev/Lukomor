using System;
using System.Reactive;
using System.Reactive.Linq;
using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class GameSessionService
    {
        public IObservable<Unit> GameOver { get; }
        public IObservable<Unit> RoundOver { get; }
        public IObservable<Unit> RoundRestarted { get; }
        public IObservable<Unit> GameRestarted { get; }
        public IReactiveProperty<int> PlayerOneScore => _playerOneScore;
        public IReactiveProperty<int> PlayerTwoScore => _playerTwoScore;
        public IReactiveProperty<bool> IsPaused => _isPaused;
        
        public PongPlayer PlayerWhoScoredLastGoal { get; private set; }
        
        private readonly int _scoreLimit;
        private readonly ReactiveProperty<int> _playerOneScore;
        private readonly ReactiveProperty<int> _playerTwoScore;
        private readonly ReactiveProperty<bool> _isPaused;
        private event Action<Unit> _gameOver;
        private event Action<Unit> _roundOver;
        private event Action<Unit> _restartedRound;
        private event Action<Unit> _restartedGame;
        
        public GameSessionService(int scoreLimit)
        {
            _scoreLimit = scoreLimit;
            _playerOneScore = new ReactiveProperty<int>(0);
            _playerTwoScore = new ReactiveProperty<int>(0);
            _isPaused = new ReactiveProperty<bool>(false);

            GameOver = Observable.FromEvent<Unit>(a => _gameOver += a, a => _gameOver -= a);
            RoundOver = Observable.FromEvent<Unit>(a => _roundOver += a, a => _roundOver -= a);
            RoundRestarted = Observable.FromEvent<Unit>(a => _restartedRound += a, a => _restartedRound -= a);
            GameRestarted = Observable.FromEvent<Unit>(a => _restartedGame += a, a => _restartedGame -= a);
        }

        public void RegisterGoal(PongPlayer winner)
        {
            if (_isPaused.Value)
            {
                return;
            }

            PlayerWhoScoredLastGoal = winner;
            
            Pause();
            
            if (winner == PongPlayer.One)
            {
                _playerOneScore.Value++;
            }
            else
            {
                _playerTwoScore.Value++;
            }
            
            if (_playerOneScore.Value >= _scoreLimit || _playerTwoScore.Value >= _scoreLimit)
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
            _playerOneScore.Value = 0;
            _playerTwoScore.Value = 0;
            
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