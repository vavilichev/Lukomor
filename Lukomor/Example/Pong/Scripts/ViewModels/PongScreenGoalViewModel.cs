using Lukomor.Reactive;

namespace Lukomor.Example.Pong
{
    public class PongScreenGoalViewModel : PongScreenViewModel
    {
        public IReactiveProperty<string> WinText => _winText;
        public IReactiveProperty<string> CountText => _countText;

        private readonly SingleReactiveProperty<string> _winText = new();
        private readonly SingleReactiveProperty<string> _countText = new();
        private readonly GameSessionsService _gameSessionsService;
        
        public PongScreenGoalViewModel(GameSessionsService gameSessionsService)
        {
            _gameSessionsService = gameSessionsService;
            _winText.Value = gameSessionsService.IsLastGoalByLeftPlayer.Value ? "Left player GOAL!" : "Right player GOAL!";
            _countText.Value = $"{gameSessionsService.LeftPlayerScore.Value}:{gameSessionsService.RightPlayerScore.Value}";
        }

        public void Continue()
        {
            _gameSessionsService.RestartRound();
        }
    }
}